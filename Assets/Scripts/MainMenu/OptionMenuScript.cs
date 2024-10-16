using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using AudioManager = Scripts.Audio.AudioManager;
using GameSettings = Scripts.Game.GameSettings;

namespace Scripts.MainMenu
{
    public class OptionMenuScript : MonoBehaviour
    {
        #region Class Variables
        [SerializeField] private UIDocument _optionMenuDocument;
        public GameObject MainMenu;
        public GameObject OptionsMenu;
        private Button _backButton, _applyChangesButton, _revertChangesButton, _increaseNPCSubtitleSpeed, _decreaseNPCSubtitleSpeed;
        private SliderInt _musicSlider, _sfxSlider, _cameraSensitivitySlider;
        private Label _musicVolumeLabel, _sfxVolumeLabel, _cameraSensitivityLabel, _npcSubtitleSpeedLabel;
        #endregion

        #region Enable Methods
        /// <summary>
        /// Calls all method when script is called upon
        /// </summary>
        private void OnEnable()
        {
            GetMenuComponents();
            SetMenuComponentEventMethods();
            SetInitialMenuComponentValues();
            DisableApplyRevertButtons();
        }

        /// <summary>
        /// Sets buttons and sliders from the option menu to a variable
        /// </summary>
        private void GetMenuComponents()
        {
            VisualElement root = _optionMenuDocument.rootVisualElement;

            //Set button fields
            _backButton = root.Q<Button>("BackButton");
            _applyChangesButton = root.Q<Button>("ApplyChangesButton");
            _revertChangesButton = root.Q<Button>("RevertChangesButton");

            _increaseNPCSubtitleSpeed = root.Q<Button>("IncreaseNPCSubtitleSpeed");
            _decreaseNPCSubtitleSpeed = root.Q<Button>("DecreaseNPCSubtitleSpeed");

            //Set slider fields
            _musicSlider = root.Q<SliderInt>("MusicSlider");
            _sfxSlider = root.Q<SliderInt>("SFXSlider");
            _cameraSensitivitySlider = root.Q<SliderInt>("CameraSensitivitySlider");

            //Set slider labels
            _musicVolumeLabel = root.Q<Label>("MusicVolumeLabel");
            _sfxVolumeLabel = root.Q<Label>("SFXVolumeLabel");
            _cameraSensitivityLabel = root.Q<Label>("CameraSensitivityLabel");
            _npcSubtitleSpeedLabel = root.Q<Label>("NPCSubtitleSpeedLabel");
        }

        /// <summary>
        /// Sets each button and slider to the appropiate method
        /// </summary>
        private void SetMenuComponentEventMethods()
        {
            //set button clicked methods
            _backButton.clickable.clicked += GoBack;
            _applyChangesButton.clickable.clicked += UpdateSettings;
            _revertChangesButton.clickable.clicked += RevertSettingsChanges;

            _increaseNPCSubtitleSpeed.clickable.clicked += IncreaseNPCSubtitleSpeed;
            _decreaseNPCSubtitleSpeed.clickable.clicked += DecreaseNPCSubtitleSpeed;

            //set slider methods
            _musicSlider.RegisterValueChangedCallback(value => UpdateSliderValue(value.newValue, _musicVolumeLabel));
            _sfxSlider.RegisterValueChangedCallback(value => UpdateSliderValue(value.newValue, _sfxVolumeLabel));
            _cameraSensitivitySlider.RegisterValueChangedCallback(value => UpdateSliderValue(value.newValue, _cameraSensitivityLabel));
        }

        /// <summary>
        /// Method for resetting settings
        /// </summary>
        private void SetInitialMenuComponentValues()
        {
            _musicSlider.SetValueWithoutNotify(Mathf.RoundToInt((AudioManager.Instance.musicSource.volume)*100));
            _sfxSlider.SetValueWithoutNotify(Mathf.RoundToInt((AudioManager.Instance.sfxSource.volume) * 100));
            _cameraSensitivitySlider.SetValueWithoutNotify(Mathf.RoundToInt((GameSettings.Instance.CameraSensitivity) * 100));

            _musicVolumeLabel.text = _musicSlider.value.ToString();
            _sfxVolumeLabel.text = _sfxSlider.value.ToString();
            _cameraSensitivityLabel.text = _cameraSensitivitySlider.value.ToString();
            _npcSubtitleSpeedLabel.text = Math.Round(GameSettings.Instance.NPCSubtitleSpeed, 1).ToString();
            CheckEnabledNPCSubtitleSpeedButtons();
        }

        /// <summary>
        /// Method for checking if npc subtitle is enabled
        /// </summary>
        private void CheckEnabledNPCSubtitleSpeedButtons()
        {
            float speed = float.Parse(_npcSubtitleSpeedLabel.text);
            if (speed > 1f)
            {
                _decreaseNPCSubtitleSpeed.SetEnabled(true);
            }
            else
            {
                _decreaseNPCSubtitleSpeed.SetEnabled(false);
            }

            if (speed < 10f)
            {
                _increaseNPCSubtitleSpeed.SetEnabled(true);
            }
            else
            {
                _increaseNPCSubtitleSpeed.SetEnabled(false);
            }
        }
        #endregion

        #region UI Component Methods
        /// <summary>
        ///Enables main menu canvas but disables option menu canvas
        /// </summary>
        private void GoBack()
        {
            OptionsMenu.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }

        /// <summary>
        /// Method for updating slider values
        /// </summary>
        private void UpdateSliderValue(float value, Label label)
        {
            if (value != -1 && label != null)
            {
                label.text = value.ToString();
                EnableApplyRevertButtons();
            }
        }

        /// <summary>
        /// Method for increasing subtitle speed
        /// </summary>
        private void IncreaseNPCSubtitleSpeed()
        {
            float speed = float.Parse(_npcSubtitleSpeedLabel.text);
            if(speed == 10f)
            {
                return;
            }
            if(speed == 1f)
            {
                _decreaseNPCSubtitleSpeed.SetEnabled(true);
            }
            speed += 0.5f;
            _npcSubtitleSpeedLabel.text = Math.Round(speed, 1).ToString();
            if(speed == 10f)
            {
                _increaseNPCSubtitleSpeed.SetEnabled(false);
            }
            EnableApplyRevertButtons();
        }

        /// <summary>
        /// Method for decreasing subtitle speed
        /// </summary>
        private void DecreaseNPCSubtitleSpeed()
        {
            float speed = float.Parse(_npcSubtitleSpeedLabel.text);
            if (speed == 1f)
            {
                return;
            }
            if (speed == 10f)
            {
                _increaseNPCSubtitleSpeed.SetEnabled(true);
            }
            speed -= 0.5f;
            _npcSubtitleSpeedLabel.text = Math.Round(speed, 1).ToString();
            if (speed == 1f)
            {
                _decreaseNPCSubtitleSpeed.SetEnabled(false);
            }
            EnableApplyRevertButtons();
        }

        /// <summary>
        /// Method for saving setting
        /// </summary>
        private void UpdateSettings()
        {
            SetMusicValue(_musicSlider.value);
            SetSFXValue(_sfxSlider.value);
            SetCameraSensitivity(_cameraSensitivitySlider.value);
            SetNPCSubtitleSpeed(float.Parse(_npcSubtitleSpeedLabel.text));
            DisableApplyRevertButtons();
        }

        /// <summary>
        /// Method for revert button
        /// </summary>
        private void RevertSettingsChanges()
        {
            SetInitialMenuComponentValues();
            DisableApplyRevertButtons();
        }
        #endregion

        #region Apply/Revert Changes Methods
        /// <summary>
        /// Method for enabling refund button
        /// </summary>
        public void EnableApplyRevertButtons()
        {
            _applyChangesButton.SetEnabled(true);
            _revertChangesButton.SetEnabled(true);
        }

        /// <summary>
        /// Method for disableing revert button
        /// </summary>
        public void DisableApplyRevertButtons()
        {
            _applyChangesButton.SetEnabled(false);
            _revertChangesButton.SetEnabled(false);
        }

        /// <summary>
        /// Method for setting SFX volume
        /// </summary>
        public void SetSFXValue(float volume)
        {
            AudioManager.Instance.SetSFXVolume(volume / 100);
        }

        /// <summary>
        /// Method for setting music volume
        /// </summary>
        public void SetMusicValue(float volume)
        {
            AudioManager.Instance.SetMusicVolume(volume / 100);
        }

        /// <summary>
        /// Method for setting camera sensetivity
        /// </summary>
        public void SetCameraSensitivity(float sensitivity)
        {
            GameSettings.Instance.SetCameraSensitivity(sensitivity / 100);
        }

        public void SetNPCSubtitleSpeed(float speed)
        {
            GameSettings.Instance.SetNPCSubtitleSpeed(speed/100);
        }
        #endregion
    }
}
