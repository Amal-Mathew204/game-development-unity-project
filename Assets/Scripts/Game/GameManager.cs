using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Class Variables
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is Null");
            }
            return _instance;
        }
    }
    public List<Mission> MissionList { get; private set; } = new List<Mission>();
    //TODO: Change/Check how GameState Object is referenced from scene
    //      (so its unaffected when scenes are changed ingame)
    [SerializeField] private GameObject _gameStateCanvas;
    #endregion

    #region Awake Methods
    private void Awake()
    {
        SetInstance();
        CreateMissions();
    }
    /// <summary>
    /// Ensures only a single instance of the GameManger class (and GameObject) is created.
    /// </summary>
    private void SetInstance()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This method creates all the Missions available for the Player
    /// </summary>
    public void CreateMissions()
    {
        MissionList = new List<Mission>() {new Mission("Slippery Slope", "Explore the terrain and locate a short, smooth hill. Reach the top of the hill."),
                                            new Mission("Find Trigger Box", "Explore the terrain and locate the trigger box. Pass under it and listen for the sound effect.")};
    }
    #endregion

    #region Update Methods
    private void Update()
    {
        if (CheckIfPlayerHasWon())
        {
            SetPlayerHasWon();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private bool CheckIfPlayerHasWon()
    {
        foreach(Mission mission in MissionList)
        {
            if(mission.IsMissionCompleted() == false)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetPlayerHasWon()
    {
        //TODO: Enable the WinPannel
        GameObject winPannel = _gameStateCanvas.transform.Find("WinPannel").gameObject;
        winPannel.gameObject.SetActive(true);



        //TODO: Change Current Action Map of Player Input to UI
        //Note: reference the game object by Player.Instance.gameObject.GetComponent<PlayerInput>();

        //TODO: (NOT IN THIS METHOD) Add a button to the pannel for WinPannel and Lose Pannel
        //      create a new script for the button that destorys the player gameobject and loads start menu.
        //      Remeber to load this new script onto the GameStateObject
    }

    private void SetPlayerHasLost()
    {
        //TODO: Change Current Action Map of Player Input to UI
        //Note: reference the game object by Player.Instance.gameObject.GetComponent<PlayerInput>();
        //TODO: Enable the LosePannel
    }
    #endregion
}
