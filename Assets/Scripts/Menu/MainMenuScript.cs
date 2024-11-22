using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.Menu
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _mainMenuDocument;
        private Button _playButton;
        private Button _settingButton;
        private Button _tutorialButton;
        public GameObject MainMenu;
        public GameObject OptionsMenu;

        /// <summary>
        ///Method obtains button components from the canvas main menu and assaigns it to a variable.
        ///Calls the correct function when a button is pressed 
        /// </summary>
        private void OnEnable()
        {
            VisualElement root = _mainMenuDocument.rootVisualElement;
            _playButton = root.Q<Button>("PlayButton");
            _settingButton = root.Q<Button>("SettingButton");
            _tutorialButton = root.Q<Button>("TutorialButton");

            //set button clicked methods
            _playButton.clickable.clicked += PlayGame;
            _tutorialButton.clickable.clicked += TutorialScreen;
            _settingButton.clickable.clicked += OptionPress;
        }

        /// <summary>
        ///Loads Game Scence
        /// </summary>
        private void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
            GameManager.Instance.DisableMouseCursor();
        }

        /// <summary>
        /// Quits the Game
        /// </summary>
        private void TutorialScreen()
        {
            Debug.Log("Tutorial Menu");
        }

        /// <summary>
        ///Loads Settings Menu
        /// </summary>
        private void OptionPress()
        {
            Debug.Log("Options Menu");
        }

    }
}


