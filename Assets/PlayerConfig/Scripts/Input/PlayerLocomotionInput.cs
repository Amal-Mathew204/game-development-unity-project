using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerConfig
{
    public class PlayerLocomotionInput : MonoBehaviour
    {
        #region Class Variables
        public bool holdToSprint = false;
        public bool holdToWalk = false;
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool isSprinting = false;
        public bool isWalking = false;
        #endregion

        #region Action Methods
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
            
        }
        public void OnWalk(InputAction.CallbackContext context)
        {

        }
        #endregion
    }
}
