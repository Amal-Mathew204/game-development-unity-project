using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using GameManager = Scripts.Game.GameManager;
using GameSettings = Scripts.Game.GameSettings;

namespace Scripts.Menu
{
    public class DifficultyMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _difficultyMenuDocument;
        private Button _backButton, _easyButton, _mediumButton, _hardButton, _startButton;
        public GameObject MainMenu;
        public GameObject DifficultyMenu;
        private string _selectedDifficultyMode = "Not Selected";

        /// <summary>
        ///Method obtains button components from the canvas difficulty menu screen and assaigns it to a variable.
        ///Calls the correct function when a button is pressed 
        /// </summary>
        private void OnEnable()
        {
            VisualElement root = _difficultyMenuDocument.rootVisualElement;

            //Set menu button fields
            _backButton = root.Q<Button>("BackButton");
            _easyButton = root.Q<Button>("EasyMode");
            _mediumButton = root.Q<Button>("MediumMode");
            _hardButton = root.Q<Button>("HardMode");
            _startButton = root.Q<Button>("StartButton");


            //By defualt start button is disabled until user selected difficulty mode
            _startButton.SetEnabled(false);

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

        /// <summary>
        /// Method for selecting Easy difficulty
        /// Also disables easy button to show user it is currently selected
        /// </summary>
        private void SelectEasyMode()
        {
            _selectedDifficultyMode = "Easy";
            _easyButton.SetEnabled(false);
            _mediumButton.SetEnabled(true);
            _hardButton.SetEnabled(true);
            _startButton.SetEnabled(true);
        }

        /// <summary>
        /// Method for selecting medium difficulty
        /// Also disables medium button to show user it is currently selected
        /// </summary>
        private void SelectMediumMode()
        {
            _selectedDifficultyMode = "Medium";
            _easyButton.SetEnabled(true);
            _mediumButton.SetEnabled(false);
            _hardButton.SetEnabled(true);
            _startButton.SetEnabled(true);
        }

        /// <summary>
        /// Method for selecting hard difficulty
        /// Also disables hard button to show user it is currently selected
        /// </summary>
        private void SelectHardMode()
        {
            _selectedDifficultyMode = "Hard";
            _easyButton.SetEnabled(true);
            _mediumButton.SetEnabled(true);
            _hardButton.SetEnabled(false);
            _startButton.SetEnabled(true);
        }


        /// <summary>
        ///Loads Game Scene
        /// </summary>
        private void PlayGame()
        {
            Debug.Log(_selectedDifficultyMode);
            GameSettings.Instance.SetDifficultyMode(_selectedDifficultyMode);

            SceneManager.LoadScene("GameScene");
            GameManager.Instance.DisableMouseCursor();
        }
    }

}
