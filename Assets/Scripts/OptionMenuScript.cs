using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class OptionMenuScript : MonoBehaviour
{
    [SerializeField] private UIDocument _optionMenuDocument;
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    private Button _BackButton;

    private void OnEnable()
    {
        VisualElement root = _optionMenuDocument.rootVisualElement;
        _BackButton = root.Q<Button>("BackButton");


        //set button clicked methods
        _BackButton.clickable.clicked += goBack;

    }

    private void goBack()
    {
        OptionsMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
    }
}
