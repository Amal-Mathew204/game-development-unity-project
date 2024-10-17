using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Scripts.MissonLogMenu;
using Scripts.Quests;
using PlayerManager = Scripts.Player.Player;
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
            //TODO: Change Current Action Map of Player Input to UI
            //Note: reference the game object by Player.Instance.gameObject.GetComponent<PlayerInput>();
        }

        /// <summary>
        /// Method Sets Game State to Player has lost and displays a Lost Game Message
        /// </summary>
        public void SetPlayerHasLost()
        {
            //TODO: Change Current Action Map of Player Input to UI
            EnablePannel("LosePannel");

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
    }

}