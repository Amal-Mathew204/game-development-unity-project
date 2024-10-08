using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerConfig
{
    public class PlayerController : MonoBehaviour
    {
        #region Class Variables
        [Header("PlayerController Components")]
        [SerializeField] private CharacterController _characterController;
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        [SerializeField] private Camera _playerCamera;

        [Header("Player Lateral Movement Settings")]
        public float walkSpeed;
        public float walkAcceleration;
        public float runSpeed;
        public float runAcceleration;
        public float sprintSpeed;
        public float sprintAcceleration;
        public float drag;

        private PlayerLocomotionState _lastLocomotionState;

        [Header("Player Vertical Movement Settings")]
        private float _verticalVelocity = 0f;
        public float gravity;
        public float jumpSpeed;
        public float inAirAcceleration;
        public float terminalVelocity;
        private float _antiBump;


        [Header("Camera Settings")]
        public float lookSensitivityH = 0.1f;
        public float lookSensitivityV = 0.1f;
        public float lookLimitV = 89f;
        private Vector2 _playerTargetRotation = Vector2.zero;
        private Vector2 _cameraRotation = Vector2.zero;

        [Header("Enviroment Settings")]
        [SerializeField] private LayerMask _groundLayers;

        //Cache Values
        private float _stepOffset;
        private bool _jumpedLastFrame; //avoid double jumping


        #endregion

        #region StartUp Methods
        void Start()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
            _stepOffset = _characterController.stepOffset;
            //set anitBump to fasted possible speed
            _antiBump = sprintSpeed;
        }
        #endregion

        #region Update Methods
        void Update()
        {
            //Note the order of these method calls matter
            UpdatePlayerLocomotionState();
            UpdatePlayerVerticalMovement();
            UpdatePlayerLateralMovement();
        }

        /// <summary>
        /// This method is to be called by the Update Unity Method.
        /// This method will update the Locomotion Movement State of the player.
        /// </summary>
        private void UpdatePlayerLocomotionState()
        {
            bool isPlayerGrounded = IsPlayerGrounded();

            //Lateral State Checks
            if (_playerLocomotionInput.MovementInput == Vector2.zero)
            {
                _playerState.CurrentLocomotionState = PlayerLocomotionState.Idling;
            }
            else if(_playerLocomotionInput.isWalking == true || CanRun() == false)
            {
                _playerState.CurrentLocomotionState = PlayerLocomotionState.Walking;

            }
            else if (_playerLocomotionInput.isSprinting == true)
            {
                _playerState.CurrentLocomotionState = PlayerLocomotionState.Sprinting;
            }
            else
            {
                _playerState.CurrentLocomotionState = PlayerLocomotionState.Running;
            }

            // Airborne State Checks
            if (!isPlayerGrounded || _jumpedLastFrame)
            {
                if (_characterController.velocity.y > 0f)
                {
                    _playerState.CurrentLocomotionState = PlayerLocomotionState.Jumping;
                }
                else if (_characterController.velocity.y < 0f)
                {
                    _playerState.CurrentLocomotionState = PlayerLocomotionState.Falling;
                }
                _jumpedLastFrame = false;
                //avoid hitching on an edge while jumping / falling
                _characterController.stepOffset = 0f;
            }
            else
            {
                _characterController.stepOffset = _stepOffset;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdatePlayerVerticalMovement()
        {
            bool isPlayerGrounded = _playerState.IsPlayerGrounded();

            _verticalVelocity -= gravity * Time.deltaTime;

            if (isPlayerGrounded && _verticalVelocity < 0)
            {
                //avoid Player to be skipping Up and Down on slops
                _verticalVelocity = - _antiBump;
            }



            if (_playerLocomotionInput.JumpPressed && isPlayerGrounded) 
            {
                // from unity docs to handle jump
                _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3f * gravity);
                //remove antiBump while airborne
                _verticalVelocity += _antiBump;
                _jumpedLastFrame = true;
            }


        }

        /// <summary>
        /// This method is to be called by the Update Unity Method.
        /// The method will update the Player Game Object velocity in response to Movement Input by the user or by drag
        /// </summary>
        private void UpdatePlayerLateralMovement()
        {

            bool isSprinting = _playerState.CurrentLocomotionState == PlayerLocomotionState.Sprinting;
            bool isWalking = _playerState.CurrentLocomotionState == PlayerLocomotionState.Walking;
            bool isGrounded = _playerState.IsPlayerGrounded();

            //Set movement speeds and acceleration
            float movementspeedMagnitude = (!isGrounded || isSprinting) ? sprintSpeed : isWalking ? walkSpeed : runSpeed;
            float lateralacceleration = !isGrounded ? inAirAcceleration : isSprinting ? sprintAcceleration : isWalking ? walkAcceleration : runAcceleration;

            //Get Normalised (Direction) Vectors of the forward (blue axis) and right (red axis) of the camera
            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;


            //Set players new velocity ensuring Players Movement is in the direction of the camera
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;
            Vector3 movementDelta = movementDirection * lateralacceleration * Time.deltaTime;
            Vector3 newPlayerVelocity = _characterController.velocity + movementDelta;

            //Adjust new velocity value with drag
            Vector3 movementDrag = newPlayerVelocity.normalized * drag * Time.deltaTime;
            //Avoid drag moving player backwards
            newPlayerVelocity = (newPlayerVelocity.magnitude > drag * Time.deltaTime) ? newPlayerVelocity - movementDrag : Vector3.zero;

            //Clamp new Velocity to corresponding max value
            newPlayerVelocity = Vector3.ClampMagnitude(new Vector3(newPlayerVelocity.x, 0f, newPlayerVelocity.z), movementspeedMagnitude);

            //add vertical movement of the player
            newPlayerVelocity.y += _verticalVelocity;

            _characterController.Move(newPlayerVelocity * Time.deltaTime);
        }
        #endregion

        #region Late Update Methods
        private void LateUpdate()
        {
            UpdateCameraRotation();
        }

        /// <summary>
        /// This method Rotates Both the Camera and Player According to the Look Input Direction provided from the user.
        /// </summary>
        private void UpdateCameraRotation()
        {
            _cameraRotation.x += lookSensitivityH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y -= lookSensitivityV * _playerLocomotionInput.LookInput.y;
             //make sure the camera rotation is clamped vertically to restrict how far up and down the player can look
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -lookLimitV, lookLimitV);

            //player must rotate with the camera (on the x plane
            _playerTargetRotation.x += transform.eulerAngles.x + lookSensitivityH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);

            // Note the Quation Euler rotates the parameter based on the direction axis.
            // To move the camera in the y direction u rotate on the horizontal axis X hence why _cameraRotation.y is in the float x parameter position
            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

        }
        #endregion
        #region State Check Methods
        /// <summary>
        /// Method Checks if the player is moving at a 45 degree forward direction.
        /// The player should not be able to run if the movement direction is not within this angle.
        /// </summary>
        /// <returns>Boolean Value</returns>
        public bool CanRun()
        {
            return _playerLocomotionInput.MovementInput.y >= Mathf.Abs(_playerLocomotionInput.MovementInput.x);
        }

        /// <summary>
        /// Method uses two checks (Checking is the player is grounded while in the ground state, or while the player is grounded while in an airborne state).
        /// </summary>
        /// <returns>Boolean Value</returns>
        public bool IsPlayerGrounded()
        {
            return _playerState.IsPlayerGrounded() ? IsGroundedWhileInGroundedState() : IsGroundedWhileInAirborneState();
        }
        /// <summary>
        /// Mathod used for checking the player is grounded when moving onto slopes. It uses a sphere collider to check the players position.
        /// </summary>
        /// <returns>Boolean Value</returns>
        public bool IsGroundedWhileInGroundedState()
        {
            // y position lower than GameObject Position to allow for a buffer between the GameObject and the ground
            Vector3 sphereColliderPosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);
            return Physics.CheckSphere(sphereColliderPosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// Method uses the character controller to see if the player has returned to the ground from being airborne.
        /// </summary>
        /// <returns>Boolean Value</returns>
        public bool IsGroundedWhileInAirborneState()
        {
            return _characterController.isGrounded;
        }
        #endregion
    }
}


