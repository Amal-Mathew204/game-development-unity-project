using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private UIDocument _mainMenuDocument;
    private Button _playButton;
    private Button _settingButton;
    private Button _quitButton;
    private void Start()
    {
        VisualElement root = _mainMenuDocument.rootVisualElement;
        _playButton = root.Q<Button>("PlayButton");
        _settingButton = root.Q<Button>("SettingButton");
        _quitButton = root.Q<Button>("QuitButton");


        //set button clicked methods
        _playButton.clickable.clicked += playGame;
        _quitButton.clickable.clicked += quitGame;

    }
    /// <summary>
    /// 
    /// </summary>
    private void playGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void quitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

