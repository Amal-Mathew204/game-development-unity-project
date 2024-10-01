using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerConfig
{
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerController Components")]
        [SerializeField] private CharacterController _characterController;
        private PlayerLocomotionInput _playerLocomotionInput;

        [Header("Player Movement Settings")]
        public float runspeed;
        public float runacceleration;
        public float drag;



        void Start()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayerLateralMovement();
        }

        private void UpdatePlayerLateralMovement()
        {
            float movementMagnitude = runspeed;
            float lateralacceleration = runacceleration;
            //set players new velocity
            Vector3 movementDirection = new Vector3(_playerLocomotionInput.MovementInput.x, 0f, _playerLocomotionInput.MovementInput.y);
            Vector3 movementDelta = movementDirection * lateralacceleration * Time.deltaTime;
            Vector3 newPlayerVelocity = _characterController.velocity + movementDelta;

            //adjust new velocity value with drag
            Vector3 movementDrag = newPlayerVelocity.normalized * drag * Time.deltaTime;
            //avoid drag moving player backwards
            newPlayerVelocity = (newPlayerVelocity.magnitude > drag * Time.deltaTime) ? newPlayerVelocity - movementDrag : Vector3.zero;

            newPlayerVelocity = Vector3.ClampMagnitude(new Vector3(newPlayerVelocity.x, 0f, newPlayerVelocity.z), movementMagnitude);

            _characterController.Move(newPlayerVelocity * Time.deltaTime);
        }
    }
}


