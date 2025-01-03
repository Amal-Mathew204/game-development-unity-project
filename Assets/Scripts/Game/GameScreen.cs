using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.Game
{
    [DefaultExecutionOrder(-1)]
    public class GameScreen : MonoBehaviour
    {
        #region Class Variables
        private static GameScreen _instance;
        public static GameScreen Instance
        {
            get
            {
                return _instance;
            }
        }
        [SerializeField] private GameObject _keyPromptTextField;
        private TextMeshProUGUI _microchipsTextField;
        private TextMeshProUGUI _timeTextField;
        [SerializeField] private GameObject _saveMessagePromptField;
        private bool _isSaveMessageDisplayed = false;
        #endregion

        #region Microchips Methods
        /// <summary>
        /// The setter method to set the `_microchipsTextField` class field
        /// </summary>
        public void SetMicrochipsTextComponent(TextMeshProUGUI textComponent)
        {
            _microchipsTextField = textComponent;
        }

        /// <summary>
        /// This method sets the microchips value to be displayed on screen
        /// </summary>
        public void UpdateMicrochipsValue(int value)
        {
            if (_microchipsTextField == null)
            {
                return;
            }
            
            _microchipsTextField.text = "Microchips: " + value.ToString();
        }
        #endregion
        
        #region Awake Methods
        private void Awake()
        {
            SetInstance();
        }
        /// <summary>
        /// Ensures only a single instance of the GameScreen class (and GameObject) is created.
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
        
        #region Key Prompt Message Field Methods
        /// <summary>
        /// This method sets the sets the KeyPromptTextField GameObject to active (making the GameObject visible)
        /// The method also sets the text (provided in the method arguement) to the KeyPromptTextField Game Object
        /// </summary>
        /// <param name="text"></param>
        public void ShowKeyPrompt(string text)
        {
            _keyPromptTextField.SetActive(true);
            TextMeshProUGUI textComponent = _keyPromptTextField.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = text;

        }

        /// <summary>
        /// This method sets the sets the KeyPromptTextField GameObject to inactive (hiding the GameObject)
        /// </summary>
        public void HideKeyPrompt()
        {
            _keyPromptTextField.SetActive(false);
        }
        #endregion
        
        #region Save Game Prompt Methods
        /// <summary>
        /// This method will display a message on screen indicating the game has been saved
        /// After 5 seconds the message will disappear
        /// </summary>
        public void DisplaySaveGamePrompt()
        {
            if (!_isSaveMessageDisplayed)
            {
                StartCoroutine(DisplaySaveGamePromptCoroutine());
            }
        }
        
        /// <summary>
        /// This is a coroutine method to display the Save Game Message Prompt Game Object for 3 seconds
        /// </summary>
        private IEnumerator DisplaySaveGamePromptCoroutine()
        {
            _isSaveMessageDisplayed = true;
            _saveMessagePromptField.SetActive(true);
            yield return new WaitForSecondsRealtime(3f);
            _saveMessagePromptField.SetActive(false);
            _isSaveMessageDisplayed = false;
        }
        #endregion
        #region Time Methods
        /// <summary>
        /// The setter method to set the `_timeTextField` class field
        /// </summary>
        public void SetTimeTextComponent(TextMeshProUGUI textComponent)
        {
            _timeTextField = textComponent;
        }

        /// <summary>
        /// This method sets the time value to be displayed on screen
        /// </summary>
        public void UpdateTimeValue(string value)
        {
            if (_timeTextField == null)
            {
                return;
            }
            
            _timeTextField.text = value;
        }
        #endregion
        
        #region Utility Methods
        /// <summary>
        /// Destroys Player GameObject
        /// </summary>
        public static void DestroyGameObject()
        {
            if (Instance == null){
                return;
            }
            Destroy(Instance.gameObject);
            _instance = null;
        }
        #endregion
    }
}