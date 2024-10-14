using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Scripts.Player.Input
{
    public class PlayerActionInput : MonoBehaviour
    {

        #region Class Variables
        [field: SerializeField] public bool IsGathering { get; private set; } = false;
        #endregion

        #region Action CallBack Methods
        public void OnGather(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            IsGathering = true;
        }
        #endregion
    }
}

