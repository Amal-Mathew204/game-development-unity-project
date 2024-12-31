using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        private Label _errorBox;
        public GameObject MainMenu;
        public GameObject OptionsMenu;
        public GameObject TutorialMenu;
        public GameObject DifficultyMenu;

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
            
            //obtain reference to errorbox
            _errorBox = root.Q<Label>("ErrorBox");
            
            //Set ErrorBox to inactive
            _errorBox.visible = false;

            //set button clicked methods
            _playButton.clickable.clicked += NewGamePress;
            _loadGameButton.clickable.clicked += LoadPress;
            _tutorialButton.clickable.clicked += TutorialPress;
            _settingButton.clickable.clicked += OptionPress;
        }

        /// <summary>
        ///Loads Difficulty Menu
        /// </summary>
        private void NewGamePress()
        {
            DifficultyMenu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
        }

        /// <summary>
        /// Loads Tutorial Menu
        /// </summary>
        private void TutorialPress()
        {
            TutorialMenu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
        }

        /// <summary>
        /// The methods loads the Game Scene and
        /// Calls the GameManager.cs method to load the games data
        /// </summary>
        private void LoadPress()
        {
            if (GameManager.Instance.CheckLoadedGameAvailable())
            {
                SceneManager.LoadScene("GameScene");
                SceneManager.sceneLoaded += LoadGameData;
            }
            else
            {
                _errorBox.visible = true;
            }
        }
        /// <summary>
        /// This method loads game data and unsubscribes from the scene-loaded event
        /// </summary>
        private void LoadGameData(Scene scene, LoadSceneMode mode)
        {
            GameManager.Instance.LoadGameData();
            SceneManager.sceneLoaded -= LoadGameData;
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


