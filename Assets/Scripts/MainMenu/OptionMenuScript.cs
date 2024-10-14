using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using AudioManager = Scripts.Audio.AudioManager;

namespace Scripts.MainMenu
{
    public class OptionMenuScript : MonoBehaviour
    {
        #region Class Variables
        [SerializeField] private UIDocument _optionMenuDocument;
        public GameObject MainMenu;
        public GameObject OptionsMenu;
        private Button _backButton;
        private Slider _musicSlider, _sfxSlider, _cameraSensitivitySlider;
        #endregion

        #region Enable Methods
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            GetMenuComponents();
            SetMenuComponentEventMethods();
            SetSliderValues();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetMenuComponents()
        {
            VisualElement root = _optionMenuDocument.rootVisualElement;

            // set button fields
            _backButton = root.Q<Button>("BackButton");

            //set slider fields
            _musicSlider = root.Q<Slider>("MusicSlider");
            _sfxSlider = root.Q<Slider>("SFXSlider");
            _cameraSensitivitySlider = root.Q<Slider>("CameraSensitivitySlider");
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMenuComponentEventMethods()
        {
            //set button clicked methods
            _backButton.clickable.clicked += GoBack;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetSliderValues()
        {
            _musicSlider.value = AudioManager.Instance.musicSource.volume;
            _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
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
        public void SetSFXValue()
        {
            float volume = _sfxSlider.value;
            AudioManager.Instance.sfxSource.volume = volume / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetMusicValue()
        {
            float volume = _musicSlider.value;
            AudioManager.Instance.musicSource.volume = volume / 100;

        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCameraSensitivity()
        {
            float sensitivity = _cameraSensitivitySlider.value;

        }
        #endregion
    }
}
