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
        private Button _loadGameButton;
        private Button _settingButton;
        private Button _tutorialButton;
        public GameObject MainMenu;
        public GameObject OptionsMenu;
        public GameObject TutorialMenu;

        /// <summary>
        ///Method obtains button components from the canvas main menu and assaigns it to a variable.
        ///Sets each buttons on click methods
        /// </summary>
        private void OnEnable()
        {
            //Obtain references to menu buttons
            VisualElement root = _mainMenuDocument.rootVisualElement;
            _playButton = root.Q<Button>("PlayButton");
            _loadGameButton = root.Q<Button>("LoadButton");
            _settingButton = root.Q<Button>("SettingButton");
            _tutorialButton = root.Q<Button>("TutorialButton");

            //set button clicked methods
            _playButton.clickable.clicked += PlayGame;
            _loadGameButton.clickable.clicked += LoadPress;
            _tutorialButton.clickable.clicked += TutorialPress;
            _settingButton.clickable.clicked += OptionPress;
        }

        /// <summary>
        ///Loads Game Scene
        /// </summary>
        private void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
            GameManager.Instance.DisableMouseCursor();
        }

        /// <summary>
        /// Loads Tutorial Menu
        /// </summary>
        private void TutorialPress()
        {
            TutorialMenu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
        }

        private void LoadPress()
        {
            Debug.Log("LoadGame");
        }

        /// <summary>
        ///Loads Settings Menu
        /// </summary>
        private void OptionPress()
        {
            OptionsMenu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
        }

    }
}


