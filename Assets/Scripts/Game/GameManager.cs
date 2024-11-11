using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Scripts.MissonLogMenu;
using Scripts.Quests;
using PlayerManager = Scripts.Player.Player;
using DropdownComponent = Scripts.MissonLogMenu.Dropdown;


namespace Scripts.Game
{
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
        public GameObject GameStateCanvas { get; set; }
        public bool HasPlayerWonGame { get; private set; }

        [Header("Game Time Settings")]
        public float GameTimeElapsed;
        [field: SerializeField] public float GameTime { get; private set; } = 60f;
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
            HasPlayerWonGame = false; //reset condition
            MissionList = new List<Mission>() {new Mission("Mini Everest", "Explore the terrain and locate a short hill. Reach the top of the hill."),
                                               new Mission("Find Trigger Box", "Explore the terrain and locate the trigger box. Pass under it and listen for the sound effect."),
                                               new CollectMission("Collect Items", "Explore the terrain. There are three items you need to collect", 3),
                                               new Mission("Fallen Hero", "Explore the terrain. There is a robot who failed its mission. Talk to it."),
                                               new Mission("Stairway", "Explore the terrain. There are some steps. Reach the top.")};
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
        /// Method Checks that Player has completed all assigned missions
        /// </summary>
        private bool CheckIfPlayerHasWon()
        {
            foreach (Mission mission in MissionList)
            {
                if (mission.IsMissionCompleted() == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method Sets Game State to Player has won and displays a Won Game Message
        /// </summary>
        private void SetPlayerHasWon()
        {
            EnablePannel("WinPannel");
            HasPlayerWonGame = true;
            EnableMouseCursor();
        }

        /// <summary>
        /// Method Sets Game State to Player has lost and displays a Lost Game Message
        /// </summary>
        public void SetPlayerHasLost()
        {
            EnablePannel("LosePannel");
            EnableMouseCursor();
        }

        /// <summary>
        /// This Method Enables either the Win or Lose Pannel in the Game and Sets the Onclick Button Method to return to Main Menu
        /// </summary>
        public void EnablePannel(string pannelName)
        {
            GameObject pannel = GameStateCanvas.transform.Find(pannelName).gameObject;
            pannel.gameObject.SetActive(true);
            Button returnToStartMenuButton = pannel.transform.Find("ReturnToStartMenuButton").gameObject.GetComponent<Button>();
            returnToStartMenuButton.onClick.AddListener(ReturnToStartMenu);
            PlayerInput playerInput = PlayerManager.Instance.gameObject.GetComponent<PlayerInput>();
            playerInput.SwitchCurrentActionMap("UI");
        }
        #endregion

        #region Change Scene Methods

        /// <summary>
        /// A Button OnClick Method that Destorys the Player Game Object, Resets Missions and Returns to Main Menu.
        /// </summary>
        public void ReturnToStartMenu()
        {
            CreateMissions(); //reset missions
            PlayerManager.DestroyGameObject();
            SceneManager.LoadScene("StartScene");
        }
        #endregion

        #region Access Game Objects Methods
        /// <summary>
        /// Throws exception if game object is not found
        /// </summary>
        public DropdownComponent GetMissionLogDropdownComponent()
        {
            DropdownComponent dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<DropdownComponent>();
            if (dropdown == null)
            {
                throw new Exception("Dropdown component not found");
            }
            else
            {
                return dropdown;
            }
        }
        #endregion

        #region Disable Methods
        /// <summary>
        /// Method Disables visability of the mouse cursor
        /// </summary>
        public void DisableMouseCursor()
        {
            Cursor.visible = false;
        }
        /// <summary>
        /// Method Enables visability of the mouse cursor
        /// </summary>
        public void EnableMouseCursor()
        {
            Cursor.visible = true;
        }
        #endregion
    }

}