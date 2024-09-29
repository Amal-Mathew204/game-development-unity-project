using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerConfig
{
    [DefaultExecutionOrder(-3)]
    public class PlayerInputManager : MonoBehaviour
    {
        private static PlayerInputManager _instance;
        public static PlayerInputManager Instance
        {
            get
            {
                return _instance;
            }
        }
        public PlayerControls PlayerControls { get; private set; }

        ///<summary>
		///When GameObject is active, check PlayerInputManager Instance has been initialised
		///</summary>
        private void Awake()
        {
            //if instance has been initialised before, return
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        ///<summary>
        ///Enables PlayerControls
        ///</summary>
        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
        }

        ///<summary>
        ///Disables PlayerControls
        ///</summary>
        private void OnDisable()
        {
            if (PlayerControls != null)
            {
                PlayerControls.Disable();
            }
        }
    }
}

