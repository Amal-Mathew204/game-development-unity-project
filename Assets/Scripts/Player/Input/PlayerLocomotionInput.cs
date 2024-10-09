using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerConfig
{
    public class PlayerLocomotionInput : MonoBehaviour
    {
        #region Class Variables
        public bool holdToSprint = true;
        public bool holdToWalk = true;
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool isSprinting = false;
        public bool isWalking = false;
        public bool JumpPressed { get; private set; }
        #endregion

        #region Action CallBack Methods
        public void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
        public void OnSprint(InputAction.CallbackContext context)
        {
            //when button is pressed
            if (context.performed)
            {
                isSprinting = holdToSprint || !isSprinting;
            }
            //when button is released from being pressed
            else if (context.canceled)
            {
                isSprinting = !holdToSprint && isSprinting;
            }
        }
        public void OnWalk(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isWalking = holdToWalk || !isWalking;
            }
            else if (context.canceled)
            {
                isWalking = !holdToWalk && isWalking;
            }
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            JumpPressed = true;
        }
        #endregion

        #region Late Update Methods
        private void LateUpdate()
        {
            JumpPressed = false;
        }
        #endregion
    }
}
