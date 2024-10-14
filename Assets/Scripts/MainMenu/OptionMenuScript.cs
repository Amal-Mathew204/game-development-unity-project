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
        private SliderInt _musicSlider, _sfxSlider, _cameraSensitivitySlider;
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
            _musicSlider = root.Q<SliderInt>("MusicSlider");
            _sfxSlider = root.Q<SliderInt>("SFXSlider");
            _cameraSensitivitySlider = root.Q<SliderInt>("CameraSensitivitySlider");
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMenuComponentEventMethods()
        {
            //set button clicked methods
            _backButton.clickable.clicked += GoBack;

            //set slider methods
            _musicSlider.RegisterValueChangedCallback(value => HandleSliderChange(value.newValue, SetMusicValue));
            _sfxSlider.RegisterValueChangedCallback(value => HandleSliderChange(value.newValue, SetMusicValue));
            _cameraSensitivitySlider.RegisterValueChangedCallback(value => HandleSliderChange(value.newValue, SetMusicValue));
        }

        /// <summary>
        /// From Google AI Overview: In C#, a delegate is a type that acts as a reference to a method with a specific parameter list and return type.
        /// </summary>
        private delegate void SetSliderValueFunction(float value = -1);

        /// <summary>
        /// 
        /// </summary>
        private void HandleSliderChange(float value, SetSliderValueFunction function)
        {
            if(value != -1 && function != null)
            {
                function(value);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetSliderValues()
        {
            _musicSlider.value = Mathf.RoundToInt((AudioManager.Instance.musicSource.volume)*100);
            _sfxSlider.value = Mathf.RoundToInt((AudioManager.Instance.sfxSource.volume) * 100);
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
        public void SetSFXValue(float volume)
        {
            AudioManager.Instance.sfxSource.volume = volume / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetMusicValue(float volume)
        {
            AudioManager.Instance.musicSource.volume = volume / 100;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCameraSensitivity(float sensitivity)
        {

        }
        #endregion
    }
}
