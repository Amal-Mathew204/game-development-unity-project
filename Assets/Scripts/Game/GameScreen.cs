using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.Game
{
    public class GameScreen : MonoBehaviour
    {
        #region Class Variables
        private static GameScreen _instance;
        public static GameScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("GameScreen is Null");
                }
                return _instance;
            }
        }
        [SerializeField] private GameObject _keyPromptTextField;

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
    }
}