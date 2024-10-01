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

        [Header("Player Movement Settings")]
        public float runspeed;
        public float runacceleration;
        public float drag;
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

            //Set players new velocity
            Vector3 movementDirection = new Vector3(_playerLocomotionInput.MovementInput.x, 0f, _playerLocomotionInput.MovementInput.y);
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
    }
}


