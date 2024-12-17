using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Menu
{
    public class TutorialMenuScript : MonoBehaviour
    {
        [SerializeField] private UIDocument _tutorialMenuDocument;
        private Button _backButton;
        public GameObject MainMenu;
        public GameObject TutorialMenu;

        private void OnEnable()
        {
            VisualElement root = _tutorialMenuDocument.rootVisualElement;
            _backButton = root.Q<Button>("BackButton");

            //set button clicked methods
            _backButton.clickable.clicked += BackToMenu;
        }

        private void BackToMenu()
        {
            TutorialMenu.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }
    }

}
