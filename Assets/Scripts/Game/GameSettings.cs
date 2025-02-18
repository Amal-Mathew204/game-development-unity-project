﻿using UnityEngine;
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
        public bool HoldToSprint { get; private set; }
        public bool HoldToWalk { get; private set; }
        public bool ShowQuestPointer {get; private set;}
        /// <summary>
        /// Event for when Player Controls are changed
        /// </summary>

        private bool _usingController;
        public bool UsingController
        {
            get
            {
                return _usingController;
            }
            private set
            {
                _usingController = value;
                GameManager.Instance?.SwitchPlayerInputDevice();
            }
        }

        /// <summary>
        /// Gives a debug error if the game settings instance is null
        /// </summary>
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

        public DifficultySettings GameDifficultySettings { get; private set; } = GameDifficultyOptions.MEDIUM;
        #endregion

        #region Awake Methods
        private void Awake()
        {
            SetInstance();
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

        #region Start Methods
        /// <summary>
        /// Method loads the player preferences
        /// </summary>
        private void Start()
        {
            CheckPlayerPrefs();
            LoadPlayerPrefs();
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

        /// <summary>
        /// Sets hold to walk so user can either choose between holding the button to walk
        /// or press the button to toggle walk
        /// </summary>
        public void SetHoldToWalk(bool holdToWalk)
        {
            HoldToWalk = holdToWalk;
            PlayerPrefs.SetInt("HoldToWalk", holdToWalk ? 1 : 0);
        }
        /// <summary>
        /// Sets hold to sprint so user can either choose between holding the button to sprint
        /// or press the button to toggle sprint
        /// </summary>
        public void SetHoldToSprint(bool holdToSprint)
        {
            HoldToSprint = holdToSprint;
            PlayerPrefs.SetInt("HoldToSprint", holdToSprint ? 1 : 0);
        }
        /// <summary>
        /// Sets whether the player is using a controller and updates the PlayerPreferences accordingly.
        /// </summary>
        public void SetUsingController(bool usingController)
        {
            UsingController = usingController;
            PlayerPrefs.SetInt("UsingController", usingController ? 1 : 0);
        }
        
        /// <summary>
        /// Sets whether the Player would like to use the Quest Pointer as a visual aid in game
        /// </summary>
        public void SetShowQuestPointer(bool showQuestPointer)
        {
            ShowQuestPointer = showQuestPointer;
            PlayerPrefs.SetInt("ShowQuestPointer", showQuestPointer ? 1 : 0);
        }
        #endregion

        #region PlayerPrefs Method
        /// <summary>
        /// Ensure Settings are stored in Player Prefs (setting their key value pairs)
        /// </summary>
        public void CheckPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("CameraSensitivity") == false)
            {
                PlayerPrefs.SetFloat("CameraSensitivity", 0.5f);
            }
            if (PlayerPrefs.HasKey("NPCSubtitleSpeed") == false)
            {
                PlayerPrefs.SetFloat("NPCSubtitleSpeed", 0.05f);
            }
            if (PlayerPrefs.HasKey("HoldToWalk") == false)
            {
                PlayerPrefs.SetInt("HoldToWalk", 1);
            }
            if (PlayerPrefs.HasKey("HoldToSprint") == false)
            {
                PlayerPrefs.SetInt("HoldToSprint", 1);
            }
            if (PlayerPrefs.HasKey("UsingController") == false)
            {
                PlayerPrefs.SetInt("UsingController", 0);
            }
            if (PlayerPrefs.HasKey("ShowQuestPointer") == false)
            {
                PlayerPrefs.SetInt("ShowQuestPointer", 1);
            }
        }
        /// <summary>
        /// Loads settings from Player Prefs
        /// </summary>
        public void LoadPlayerPrefs()
        {
            CameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
            NPCSubtitleSpeed = PlayerPrefs.GetFloat("NPCSubtitleSpeed");
            HoldToWalk = PlayerPrefs.GetInt("HoldToWalk") == 1;
            HoldToSprint = PlayerPrefs.GetInt("HoldToSprint") == 1;
            UsingController = PlayerPrefs.GetInt("UsingController") == 1;
            ShowQuestPointer = PlayerPrefs.GetInt("ShowQuestPointer") == 1;
            //check NPCSubtitleSpeed
            if(NPCSubtitleSpeed > 0.1f)
            {
                NPCSubtitleSpeed = 0.1f;
            }

            if (NPCSubtitleSpeed < 0.01f)
            {
                NPCSubtitleSpeed = 0.01f;
            }

            //Check Camera Sensitivity
            if (CameraSensitivity < 0)
            {
                CameraSensitivity = 0;
            }
            if (CameraSensitivity > 1)
            {
                CameraSensitivity = 1;
            }
        }
        #endregion

        #region Difficulty Mode Methods

        /// <summary>
        /// Method for setting difficulty mode
        /// </summary>
        public void SetDifficultyMode(string mode)
        {
            if (Enum.IsDefined(typeof(DifficultyModes), mode))
            { 
                GameDifficultySettings = GameDifficultyOptions.GetDifficultyModeSettings(Enum.Parse<DifficultyModes>(mode));
            }
            else
            {
                Debug.LogError("Invalid Difficulty Mode");
            }
        }
        #endregion
    }
}

