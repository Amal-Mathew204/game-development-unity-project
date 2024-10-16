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
        /// Checks duplication
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
        /// Sets camera sentivity ensuring user does not go above or below limit
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
        /// Sets NPC subtittle ensuring user does not go above or below limit
        /// </summary>
        public void SetNPCSubtitleSpeed(float speed)
        {
            if (speed < 0.01f || speed > 0.1f)
            {
                throw new ArgumentOutOfRangeException("Subtitle Speed value must be between 0.01 to 0.1");
            }
            NPCSubtitleSpeed = speed;
            PlayerPrefs.SetFloat("NPCSubtitleSpeed", speed);
        }
        #endregion

        #region PlayerPrefs Method
        /// <summary>
        /// Checks player preferences
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
        /// Loads player preferences
        /// </summary>
        public void LoadPlayerPrefs()
        {
            CameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
            NPCSubtitleSpeed = PlayerPrefs.GetFloat("NPCSubtitleSpeed");
        }
        #endregion
    }
}

