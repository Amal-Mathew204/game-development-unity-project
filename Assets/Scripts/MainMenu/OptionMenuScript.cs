using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scripts.MainMenu
{
    public class OptionMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _optionMenuDocument;
        public GameObject MainMenu;
        public GameObject OptionsMenu;
        private Button _BackButton;

        /// <summary>
        /// Assigns back button and sets onclick method for it.
        /// </summary>
        private void OnEnable()
        {
            VisualElement root = _optionMenuDocument.rootVisualElement;
            _BackButton = root.Q<Button>("BackButton");

            //set button clicked methods
            _BackButton.clickable.clicked += GoBack;

        }

        /// <summary>
        ///Enables main menu canvas but disables option menu canvas
        /// </summary>
        private void GoBack()
        {
            OptionsMenu.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }

    }
}
