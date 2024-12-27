using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.Menu
{
    public class DifficultyMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _difficultyMenuDocument;
        private Button _backButton, _easyButton, _mediumButton, _hardButton, _startButton;
        public GameObject MainMenu;
        public GameObject DifficultyMenu;

        /// <summary>
        ///Method obtains button components from the canvas difficulty menu screen and assaigns it to a variable.
        ///Calls the correct function when a button is pressed 
        /// </summary>
        private void OnEnable()
        {
            VisualElement root = _difficultyMenuDocument.rootVisualElement;
            _backButton = root.Q<Button>("BackButton");
            _easyButton = root.Q<Button>("EasyMode");
            _mediumButton = root.Q<Button>("MediumMode");
            _hardButton = root.Q<Button>("HardMode");
            _startButton = root.Q<Button>("StartButton");

            //set button clicked methods
            _backButton.clickable.clicked += BackToMenu;
            _easyButton.clickable.clicked += SelectEasyMode;
            _mediumButton.clickable.clicked += SelectMediumMode;
            _hardButton.clickable.clicked += SelectHardMode;
            _startButton.clickable.clicked += PlayGame;
        }

        /// <summary>
        /// Loads Main Menu
        /// </summary>
        private void BackToMenu()
        {
            DifficultyMenu.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }


        private void SelectEasyMode()
        {
            _easyButton.SetEnabled(false);
            _mediumButton.SetEnabled(true);
            _hardButton.SetEnabled(true);
        }

        private void SelectMediumMode()
        {
            _easyButton.SetEnabled(true);
            _mediumButton.SetEnabled(false);
            _hardButton.SetEnabled(true);
        }

        private void SelectHardMode()
        {
            _easyButton.SetEnabled(true);
            _mediumButton.SetEnabled(true);
            _hardButton.SetEnabled(false);
        }


        /// <summary>
        ///Loads Game Scene
        /// </summary>
        private void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
            GameManager.Instance.DisableMouseCursor();
        }
    }

}
