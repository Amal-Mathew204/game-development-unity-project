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
        private GameState _gameState;
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
            //TODO: The trigger box missions will no longer work. To utlilise these please consult the BuildFarm Quest to change the Quest Script Logic.
            //MissionList = new List<Mission>() {new Mission("Mini Everest", "Explore the terrain and locate a short hill. Reach the top of the hill."),
            //                               new Mission("Find Trigger Box", "Explore the terrain and locate the trigger box. Pass under it and listen for the sound effect."),
            //                               new CollectMission("Collect Item", "Explore the terrain. There are three items you need to collect", 3),
            //                               new Mission("Fallen Hero", "Explore the terrain. There is a robot who failed its mission. Talk to it."),
            //                               new Mission("Stairway", "Explore the terrain. There are some steps. Reach the top."),
            //                               new Mission("Create Farm", "Find the tools and land to greenify the world ")};

            HasPlayerWonGame = false; //reset condition

            // Creates new mission called create Farm and collect/sub missions associated with it 
            Mission farmMission = new Mission("Create Farm", "Build the farm by collecting items required and planting seeds");
            CollectMission collectShovel = new CollectMission("Find Shovel", "Explore the terrian to locate the shovel which is needed for farming", 1, new List<string>() { "Shovel" });
            CollectMission collectSeedBag = new CollectMission("Find SeedBags", "Explore the terrian to salvage the seed bags", 3, new List<string>() { "Seed Bag" });
            CollectMission cleanUp = new CollectMission("Clean Up", "Clean up all the oil barrels around the map ", 2, new List<string>() { "Barrel" });
            Mission wasteBarrel = new Mission("Place Barrel in Container", "Place all barrel in container");
            Mission findFarmLand = new Mission("Find Farm Land", "");
            Mission buildFarm = new Mission("Build Farm", "Using the shovel build an area to plant some vegetation");
            Mission plantSeed = new Mission("Plant Seed", "Drop the seed bags in the farmland ");

            CollectMission collectGPSScanner = new CollectMission("Find Gps Scanner", "Explore the terrian to locate the gps scanner which is needed to locate a water source", 1, new List<string>() { "GPS Scanner" });
            CollectMission collectDynamite = new CollectMission("Find dynamite", "Explore the terrian to locate the dynamite which is needed to blow up the entrance to the water source", 1, new List<string>() { "Dynamite" });

            Mission findWaterCave = new Mission("Water Source Location", "Use the gps scanner to locate a water source and the dynamite to blow up the cave entrance");
            Mission blowUpEntrance = new Mission("Blow Up Entrance", "Use the dynamite to blow up the water cave's entrance");

            Mission findWater = new Mission("Find Water", "In order to grow crops on this desolate land we need to find a water source ");

            findWater.AddSubMission(new List<Mission>() { collectGPSScanner, collectDynamite, findWaterCave, blowUpEntrance });




            ////Adds sub-mission to the farm mission 
            farmMission.AddSubMission(collectShovel);
            farmMission.AddSubMission(collectSeedBag);
            //farmMission.AddSubMission(findFarmLand);
            farmMission.AddSubMission(buildFarm);

            MissionList = new List<Mission>() {farmMission, cleanUp, 
                                               plantSeed, wasteBarrel,
                                               findWater};
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

        #region Game Screen Methods
        /// <summary>
        /// This method is used to toggle the visibility of the enter text field
        /// This method allows the player to be acknowledged that they need to press enter to move to the next line
        /// Called through the player script
        /// </summary>
        public void ChangeEnterTextFieldVisibility(bool visibility)
        {
            GameObject gameScreenCanvas = GameObject.FindGameObjectWithTag("GameScreen");
            if(gameScreenCanvas == null)
            {
                Debug.LogError("Game Screen could not be found");
                return;
            }
            GameObject enterTextField = gameScreenCanvas.transform.Find("EnterTextField").gameObject;
            enterTextField.SetActive(visibility);
        }
        #endregion

        #region Game State Methods
        /// <summary>
        /// This method is called to reduce the battery level of the player by a percentage
        /// </summary>
        public void SetBatteryLevelReduction(float percentageReduction)
        {
            _gameState.SetBatteryLevelReduction(percentageReduction);
        }

        /// <summary>
        /// The method allows the GameState class to set a reference to itself through the Game Manager Singleton Instance
        /// </summary>
        public void SetGameState(GameState state)
        {
            _gameState = state;
        }
        #endregion

        #region Mission Methods
        public void SetMissionComplete(string missionTitle)
        {
            Mission mission = GetMission(missionTitle);
            if(mission == null)
            {
                Debug.LogError($"Mission {missionTitle} was not found");
                return;
            }
            mission.SetMissionCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        private Mission GetMission(string MissionTitle)
        {
            foreach (Mission mission in MissionList)
            {
                if (mission.hasSubMissions())
                {
                    foreach (Mission subMission in mission.SubMissions)
                    {
                        if (subMission.MissionTitle == MissionTitle)
                        {
                            return subMission;
                        }
                    }
                }
                else
                {
                    if (mission.MissionTitle == MissionTitle)
                    {
                        return mission;
                    }
                }
            }
            return null;
        }
        #endregion
    }

}