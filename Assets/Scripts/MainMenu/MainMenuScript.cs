using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scripts.MainMenu
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _mainMenuDocument;
        private Button _playButton;
        private Button _settingButton;
        private Button _quitButton;
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
            _quitButton = root.Q<Button>("QuitButton");

            //set button clicked methods
            _playButton.clickable.clicked += PlayGame;
            _quitButton.clickable.clicked += QuitGame;
            _settingButton.clickable.clicked += OptionPress;
        }

        /// <summary>
        ///Loads Game Scence
        /// </summary>
        private void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        /// <summary>
        /// Quits the Game
        /// </summary>
        private void QuitGame()
        {
            Debug.Log("QUIT");
            Application.Quit();
        }

        /// <summary>
        ///Disabled main menu canvas but enables option menu canvas
        /// </summary>
        private void OptionPress()
        {
            MainMenu.gameObject.SetActive(false);
            OptionsMenu.gameObject.SetActive(true);
        }

    }
}


