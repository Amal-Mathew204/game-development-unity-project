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
        [SerializeField] private Camera _playerCamera;

        [Header("Player Movement Settings")]
        public float runspeed;
        public float runacceleration;
        public float drag;

        [Header("Camera Settings")]
        public float lookSensitivityH = 0.1f;
        public float lookSensitivityV = 0.1f;
        public float lookLimitV = 89f;
        private Vector2 _playerTargetRotation = Vector2.zero;
        private Vector2 _cameraRotation = Vector2.zero;
        #endregion

        #region StartUp Methods
        void Start()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }
        #endregion

        #region Update Methods
        void Update()
        {
            UpdatePlayerLateralMovement();
        }

        /// <summary>
        /// This method is to be called by the Update Unity Method.
        /// The method will update the Player Game Object velocity in response to Movement Input by the user or by drag
        /// </summary>
        private void UpdatePlayerLateralMovement()
        {
            //Set movement speeds and acceleration
            float movementspeedMagnitude = runspeed;
            float lateralacceleration = runacceleration;

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
    }
}


