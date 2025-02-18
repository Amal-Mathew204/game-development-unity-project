using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player.Input
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
        
        #region Start Methods
        /// <summary>
        /// This method applies the Player Locomotion Settings from the Game Settings Singleton
        /// </summary>
        private void Start()
        {
            if (GameSettings.Instance != null)
            {
             //Set holdToSprint and holdToWalk variables
             holdToSprint = GameSettings.Instance.HoldToSprint;
             holdToWalk = GameSettings.Instance.HoldToWalk;
            }
        }

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
