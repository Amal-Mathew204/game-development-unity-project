using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scripts.Game
{
    public class GameSettings : MonoBehaviour
    {
        #region Class Variables
        private static GameSettings _instance;
        public float CameraSensitivity { get; private set; }
        public float NPCSubtitleSpeed { get; private set; }

        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Game Settings is Null");
                }
                return _instance;
            }
        }
        #endregion

        #region Awake Methods
        private void Awake()
        {
            SetInstance();
            CheckPlayerPrefs();
            LoadPlayerPrefs();
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetInstance()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region GameSettings Methods

        /// <summary>
        /// 
        /// </summary>
        public void SetCameraSensitivity(float sensitivity)
        {
            if (sensitivity < 0 || sensitivity > 1)
            {
                throw new ArgumentOutOfRangeException("Sensitivity value must be between 0 to 1");
            }
            CameraSensitivity = sensitivity;
            PlayerPrefs.SetFloat("CameraSensitivity", sensitivity);

        }

        /// <summary>
        /// 
        /// </summary>
        public void SetNPCSubtitleSpeed(float speed)
        {
            if (speed < 1 || speed > 10)
            {
                throw new ArgumentOutOfRangeException("Subtitle Speed value must be between 1 to 10");
            }
            NPCSubtitleSpeed = speed;
            PlayerPrefs.SetFloat("NPCSubtitleSpeed", speed);
        }
        #endregion

        #region PlayerPrefs Method
        /// <summary>
        /// 
        /// </summary>
        public void CheckPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("CameraSensitivity") == false)
            {
                PlayerPrefs.SetFloat("CameraSensitivity", 0.5f);
            }
            if (PlayerPrefs.HasKey("NPCSubtitleSpeed") == false)
            {
                PlayerPrefs.SetFloat("NPCSubtitleSpeed", 1.5f);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void LoadPlayerPrefs()
        {
            CameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
            NPCSubtitleSpeed = PlayerPrefs.GetFloat("NPCSubtitleSpeed");
        }
        #endregion
    }
}

