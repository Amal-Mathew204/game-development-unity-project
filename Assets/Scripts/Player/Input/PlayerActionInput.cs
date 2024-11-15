using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Scripts.Player.Input
{
    public class PlayerActionInput : MonoBehaviour
    {

        #region Class Variables
        [field: SerializeField] public bool IsGathering { get; private set; } = false;
        [field: SerializeField] public bool IsPressingAcceptKey { get; private set; } = false;
        private float _acceptTimer = 0f;
        [SerializeField] private float _acceptTimerLimit = 3f;
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

        public void OnAccept(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            IsPressingAcceptKey = true;
            _acceptTimer = 0f;
        }
        #endregion


        # region Update
        private void Update()
        {
            if (IsPressingAcceptKey)
            {
                _acceptTimer += Time.deltaTime;
            }
            if(_acceptTimer > _acceptTimerLimit)
            {
                _acceptTimer = 0;
                IsPressingAcceptKey = false;
            }
        }
        #endregion

        #region Late Update Methods
        private void LateUpdate()
        {
            IsGathering = false;
            IsPressingAcceptKey = false;
        }
        #endregion
    }
}

