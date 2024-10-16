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
}
