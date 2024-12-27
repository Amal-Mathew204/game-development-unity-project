using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Menu
{
    public class DifficultyMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _difficultyMenuDocument;
        private Button _backButton;
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

            //set button clicked methods
            _backButton.clickable.clicked += BackToMenu;
        }

        /// <summary>
        /// Loads Main Menu
        /// </summary>
        private void BackToMenu()
        {
            DifficultyMenu.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }
    }

}
