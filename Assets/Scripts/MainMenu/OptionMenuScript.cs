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
        private Button _backButton, _applyChangesButton, _revertChangesButton;
        private SliderInt _musicSlider, _sfxSlider, _cameraSensitivitySlider;
        private Label _musicVolumeLabel, _sfxVolumeLabel, __cameraSensitivityLabel;
        #endregion

        #region Enable Methods
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            GetMenuComponents();
            SetInitialMenuComponentValues();
            SetMenuComponentEventMethods();
            DisableApplyRevertButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetMenuComponents()
        {
            VisualElement root = _optionMenuDocument.rootVisualElement;

            //Set button fields
            _backButton = root.Q<Button>("BackButton");
            _applyChangesButton = root.Q<Button>("ApplyChangesButton");
            _revertChangesButton = root.Q<Button>("RevertChangesButton");

            //Set slider fields
            _musicSlider = root.Q<SliderInt>("MusicSlider");
            _sfxSlider = root.Q<SliderInt>("SFXSlider");
            _cameraSensitivitySlider = root.Q<SliderInt>("CameraSensitivitySlider");

            //Set slider labels
            _musicVolumeLabel = root.Q<Label>("MusicVolumeLabel");
            _sfxVolumeLabel = root.Q<Label>("SFXVolumeLabel");
            __cameraSensitivityLabel = root.Q<Label>("CameraSensitivityLabel");
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMenuComponentEventMethods()
        {
            //set button clicked methods
            _backButton.clickable.clicked += GoBack;
            _applyChangesButton.clickable.clicked += UpdateSettings;
            _revertChangesButton.clickable.clicked += RevertSettingsChanges;
            //set slider methods
            _musicSlider.RegisterValueChangedCallback(value => UpdateSliderValue(value.newValue, _musicVolumeLabel));
            _sfxSlider.RegisterValueChangedCallback(value => UpdateSliderValue(value.newValue, _sfxVolumeLabel));
            _cameraSensitivitySlider.RegisterValueChangedCallback(value => UpdateSliderValue(value.newValue, __cameraSensitivityLabel));
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetInitialMenuComponentValues()
        {
            _musicSlider.value = Mathf.RoundToInt((AudioManager.Instance.musicSource.volume)*100);
            _sfxSlider.value = Mathf.RoundToInt((AudioManager.Instance.sfxSource.volume) * 100);
            _cameraSensitivitySlider.value = Mathf.RoundToInt((GameSettings.Instance.CameraSensitivity) * 100);

            _musicVolumeLabel.text = _musicSlider.value.ToString();
            _sfxVolumeLabel.text = _sfxSlider.value.ToString();
            __cameraSensitivityLabel.text = _cameraSensitivitySlider.value.ToString();
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
        /// 
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
        /// 
        /// </summary>
        private void UpdateSettings()
        {
            SetMusicValue(_musicSlider.value);
            SetSFXValue(_sfxSlider.value);
            SetCameraSensitivity(_cameraSensitivitySlider.value);
            DisableApplyRevertButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RevertSettingsChanges()
        {
            SetInitialMenuComponentValues();
            DisableApplyRevertButtons();
        }
        #endregion

        #region Apply/Revert Changes Methods
        /// <summary>
        /// 
        /// </summary>
        public void EnableApplyRevertButtons()
        {
            _applyChangesButton.SetEnabled(true);
            _revertChangesButton.SetEnabled(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisableApplyRevertButtons()
        {
            _applyChangesButton.SetEnabled(false);
            _revertChangesButton.SetEnabled(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetSFXValue(float volume)
        {
            AudioManager.Instance.SetSFXVolume(volume / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetMusicValue(float volume)
        {
            AudioManager.Instance.SetMusicVolume(volume / 100);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCameraSensitivity(float sensitivity)
        {
            GameSettings.Instance.SetCameraSensitivity(sensitivity / 100);
        }
        #endregion
    }
}
