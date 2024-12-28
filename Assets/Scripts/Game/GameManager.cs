using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Audio;
using Scripts.Item;
using Scripts.Lighting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Scripts.Quests;
using PlayerManager = Scripts.Player.Player;




namespace Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        #region Class Variables
        [SerializeField] private GameObject _virtualMouse;
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
        public bool HasGameEnded { get; private set; } = false;
        private GameState _gameState;
        [Header("Game Time Properties")]
        public float GameTimeElapsed;
        private bool _usingController = false;
        public bool LoadedGame { get; private set; } = false;
        #endregion

        #region Awake Methods
        private void Awake()
        {
            SetInstance();
            CreateMissions();
            TrackPlayerControls();
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
            CollectMission collectSeedBag = new CollectMission("Find SeedBags", "Explore the terrian to salvage the seed bags", 4, new List<string>() { "Seed Bag" });
            CollectMission cleanUp = new CollectMission("Clean Up", "Clean up all the oil barrels around the map ", 5, new List<string>() { "Barrel" });
            CollectMission collectFuelCell = new CollectMission("Collect Fuell Cell", "Collect all fuel cells to turn on the generator ", 3, new List<string>() { "Fuel Cell" });
            Mission wasteBarrel = new Mission("Place Barrels in Container", "Place all barrels in container");
            Mission findFarmLand = new Mission("Find Farm Land", "");
            Mission buildFarm = new Mission("Build Farm", "Using the shovel build an area to plant some vegetation");
            Mission plantSeed = new Mission("Plant Seed", "Drop the seed bags in the farmland ");

            CollectMission collectGPSScanner = new CollectMission("Find Gps Scanner", "Explore the terrian to locate the gps scanner which is needed to locate a water source", 1, new List<string>() { "GPS Scanner" });
            CollectMission collectDynamite = new CollectMission("Find dynamite", "Explore the terrian to locate the dynamite which is needed to blow up the entrance to the water source", 1, new List<string>() { "Dynamite" });

            Mission findWaterCave = new Mission("Water Source Location", "Use the gps scanner to locate a water source and the dynamite to blow up the cave entrance");
            Mission blowUpEntrance = new Mission("Blow Up Entrance", "Use the dynamite to blow up the water cave's entrance");

            Mission findWater = new Mission("Find Water", "In order to grow crops on this desolate land we need to find a water source ");

            Mission restorePower = new Mission("Restore Power", "Find the fuell cells to restore power ");

            Mission TurnOnGenerator = new Mission("Turn on Generator", "Use the fuel cells and drop them onto the generator to turn on the power");

            findWater.AddSubMission(new List<Mission>() { collectGPSScanner, collectDynamite, findWaterCave, blowUpEntrance});
            restorePower.AddSubMission(new List<Mission>() {collectFuelCell, TurnOnGenerator });

            ////Adds sub-mission to the farm mission 
            farmMission.AddSubMission(collectShovel);
            farmMission.AddSubMission(collectSeedBag);
            //farmMission.AddSubMission(findFarmLand);
            farmMission.AddSubMission(buildFarm);

            MissionList = new List<Mission>() {farmMission, cleanUp, 
                                               plantSeed, wasteBarrel,
                                               findWater,restorePower};
        }

        /// <summary>
        /// Tracks changes in player controls and switches the input device accordingly
        /// </summary>
        private void TrackPlayerControls()
        {
            GameSettings.OnControlsChanged += SwitchPlayerInputDevice;
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Calls the method required to start the panic attack
        /// only calls method if all the missions have been completed
        /// </summary>
        private void Update()
        {
            if (CheckIfPlayerHasWon() && !HasGameEnded && !HasPlayerWonGame) 
            {
                TriggerPanic();
                HasGameEnded = true;  
            }
        }

        /// <summary>
        /// Starts the panic attack sequence 
        /// </summary>
        public void TriggerPanic()
        {
            PanicTrigger panicTrigger = FindObjectOfType<PanicTrigger>(); // Ensure there's a PanicTrigger in the scene
            if (panicTrigger != null)
            {
                panicTrigger.StartPanic();
            }
            else
            {
                Debug.LogError("No PanicTrigger found in the scene.");
            }
        }

        /// <summary>
        /// Method Checks that Player has completed all assigned missions
        /// </summary>
        private bool CheckIfPlayerHasWon()
        {
            foreach (Mission mission in MissionList)
            {
                if (!mission.IsMissionCompleted())
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Method Sets Game State to Player has won and displays a Won Game Message
        /// </summary>
        public void SetPlayerHasWon()
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
            HasGameEnded = true;
        }
        #endregion

        #region Change Scene Methods

        /// <summary>
        /// A Button OnClick Method that Destorys the Player Game Object, Resets Missions and Returns to Main Menu.
        /// </summary>
        public void ReturnToStartMenu()
        {
            CreateMissions(); //reset missions
            AudioManager.Instance.StopSFXLoop();
            PlayerManager.DestroyGameObject();
            GameScreen.DestroyGameObject();
            SceneManager.LoadScene("StartScene");
            HasGameEnded = false;
        }
        #endregion

        #region Disable Methods
        /// <summary>
        /// Method Disables visability of the cursor
        /// </summary>
        public void DisableMouseCursor()
        {
            if (_usingController)
            {
                _virtualMouse.SetActive(false);
                return;
            }
            Cursor.visible = false;

        }
        /// <summary>
        /// Method Enables visability of the cursor
        /// </summary>
        public void EnableMouseCursor()
        {
            if (_usingController)
            {
                _virtualMouse.SetActive(true);
                return;
            }
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
        /// This method is called to increase the battery level
        /// </summary>
        public void SetBatteryLevelIncrease(float batteryLevelIncreaseValue)
        {
            _gameState.SetBatteryLevelIncrease(batteryLevelIncreaseValue);
        }
        
        /// <summary>
        /// The method allows the GameState class to set a reference to itself through the Game Manager Singleton Instance
        /// </summary>
        public void SetGameState(GameState state)
        {
            _gameState = state;
        }
        
        /// <summary>
        /// Used to reference Game State in other scripts
        /// </summary>
        public GameState GetGameState()
        {
            return _gameState;
        }
        #endregion

        #region Mission Methods
        /// <summary>
        /// Marks the specified mission as completed
        /// Retrieves the mission by its title and sets it as completed
        /// </summary>
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
        /// Retrieves a mission by its title from the list of missions
        /// Searches both main missions and their sub-missions
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

        #region Switch Player Input Device Methods
        /// <summary>
        /// Switches the player's input device based on the current settings.
        /// Switches the cursors depending on the input device.
        /// </summary>
        private void SwitchPlayerInputDevice()
        {
            _usingController = GameSettings.Instance.UsingController;
            if (_usingController)
            {
                Cursor.visible = false;
                _virtualMouse.SetActive(true);
            }
            else
            {
                Cursor.visible = true;
                _virtualMouse.SetActive(false);
            }
        }
        #endregion

        #region Save/Load Methods
        /// <summary>
        /// This method saves all game data inside the game
        /// This includes the following properties:
        /// battery level, player position, time of day, microchips held, mission list, items in inventory
        /// </summary>
        public void SaveGame()
        {
            //Data Collection
            string playerPositionToSave = PlayerManager.Instance.transform.position.ToString();
            List<string> playerInventory = new List<string>();
            //Collect Name of Items in inventory
            foreach (ItemPickup item in PlayerManager.Instance.Inventory)
            {
                playerInventory.Add(item.gameObject.name);
            }
            string playerInventoryToSave =JsonConvert.SerializeObject(playerInventory);
            int microChipsToSave = PlayerManager.Instance.MicroChips;
            float gameElapsedTimeToSave = _gameState.GetGameElapsedTime();
            //Collect Mission Data 
            Dictionary<string, Dictionary<string, string>> missionsListDictionary = new Dictionary<string, Dictionary<string, string>>();
            //Serializing missions data
            //save the data from a Mission Object Inside of a Dictionary
            //The dictionaries will be stored into a list and serialised once all mission data is collected 
            foreach (Mission mission in MissionList)
            {
                
                Dictionary<string, string> missionData = new Dictionary<string, string>();
                missionData.Add("completed", mission.IsMissionCompleted().ToString());
                
                //If a collect mission save number of collected items
                if (mission.GetType().Name == "CollectMission")
                {
                    CollectMission collectMission = (CollectMission)mission;
                    missionData.Add("collectedItems", collectMission.GetCollectedItems().ToString());
                }
                
                if (mission.hasSubMissions())
                {
                    missionData.Add("submissions", mission.SubMissions.Count.ToString());
                    missionsListDictionary.Add(mission.MissionTitle, missionData);
                    //String Serialize and Save Sub Mission Data
                    foreach (Mission subMission in mission.SubMissions)
                    {
                        Dictionary<string, string> submissionData = new Dictionary<string, string>();
                        submissionData.Add("completed", subMission.IsMissionCompleted().ToString());
                        //If a collect mission save number of collected items
                        if (subMission.GetType().Name == "CollectMission")
                        {
                            CollectMission collectMission = (CollectMission)subMission;
                            submissionData.Add("collectedItems", collectMission.GetCollectedItems().ToString());
                        }
                        missionsListDictionary.Add(subMission.MissionTitle, submissionData);
                    }
                }
                else
                {
                    missionsListDictionary.Add(mission.MissionTitle, missionData);
                }
            }
            string missionsToSave = JsonConvert.SerializeObject(missionsListDictionary);
            string currentTimeToSave = GameObject.Find("LightController").GetComponent<LightingController>().GetCurrentTime().ToString();
            
            Debug.Log("Player Position: " + playerPositionToSave);
            Debug.Log("Player Inventory: " + playerInventoryToSave);
            Debug.Log("Player Microchips: " + microChipsToSave);
            Debug.Log("Current Time: " + currentTimeToSave);
            Debug.Log("Game Elapsed Time: " + gameElapsedTimeToSave );
            Debug.Log("Missions: " + missionsToSave);
            
            //Save Data To Player Prefs
            PlayerPrefs.SetString("PlayerPosition", playerPositionToSave);
            PlayerPrefs.SetString("PlayerInventory", playerInventoryToSave);
            PlayerPrefs.SetInt("PlayerMicrochips", microChipsToSave);
            PlayerPrefs.SetString("CurrentTime", currentTimeToSave);
            PlayerPrefs.SetFloat("GameElapsedTime", gameElapsedTimeToSave);
            PlayerPrefs.SetString("Missions", missionsToSave);
        }
        
        /// <summary>
        /// This method loads game data from Player Prefs and Set it in the Game
        /// </summary>
        public void LoadGameData()
        {
            //Set Loaded Game to True
            LoadedGame = true;
            //Get Data from Player Prefs
            string playerPositionToLoad = PlayerPrefs.GetString("PlayerPosition");
            string playerInventoryToLoad = PlayerPrefs.GetString("PlayerInventory");
            int playerMicrochips = PlayerPrefs.GetInt("PlayerMicrochips");
            string currentTimeToLoad = PlayerPrefs.GetString("CurrentTime");
            float gameElapsedTime = PlayerPrefs.GetFloat("GameElapsedTime");
            string missionsToLoad = PlayerPrefs.GetString("Missions");
            
            //Obtain values from serialised fields
            Vector3 playerPosition = StringToVector3(playerPositionToLoad);
            List<string> playerInventory = JsonConvert.DeserializeObject<List<string>>(playerInventoryToLoad);
            DateTime currentTime = DateTime.Parse(currentTimeToLoad);
            Dictionary<string, Dictionary<string, string>> missionsDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(missionsToLoad);
            
            //Load Player Data
            PlayerManager.Instance.SetLoadedPlayerData(playerPosition, playerMicrochips);
            
            //Load Player Inventory
            foreach (String itemGameObjectName in playerInventory)
            {
                GameObject itemGameObject = GameObject.Find(itemGameObjectName);
                itemGameObject.GetComponent<ItemPickup>().ProcessPickUp();
            }
            //Set Game Current Time and Elapsed Time
            GameObject.Find("LightController").GetComponent<LightingController>().SetCurrentTime(currentTime);
            GameTimeElapsed = gameElapsedTime;
            
            //set missions to state of loaded game
            foreach (Mission mission in MissionList)
            {
                if (missionsDict[mission.MissionTitle]["completed"].Equals("True"))
                {
                    mission.SetMissionCompleted();
                }
                //If a collect mission load number of collected items
                if (mission.GetType().Name == "CollectMission")
                {
                    CollectMission collectMission = (CollectMission)mission;
                    collectMission.SetCollectedItems(Convert.ToInt32(missionsDict[mission.MissionTitle]["collectedItems"]));
                }

                if (mission.hasSubMissions())
                {
                    foreach (Mission subMission in mission.SubMissions)
                    {
                        if (missionsDict[subMission.MissionTitle]["completed"].Equals("True"))
                        {
                            subMission.SetMissionCompleted();
                        }
                        //If a collect mission load number of collected items
                        if (subMission.GetType().Name == "CollectMission")
                        {
                            CollectMission collectMission = (CollectMission)subMission;
                            collectMission.SetCollectedItems(Convert.ToInt32(missionsDict[subMission.MissionTitle]["collectedItems"]));
                        }
                    }
                }
            }
            

            //TODO: Delete Objects from game
            //TODO: Set the State of Mission Prefabs
            
        }
        #endregion
        
        #region Utility Methods
        /// <summary>
        /// Takes a string in format (x,y,z) and returns a vector 3 value
        /// </summary>
        public Vector3 StringToVector3(string value)
        {
            value = value.Replace("(", "");
            value = value.Replace(")", "");
            string[] values = value.Split(',');
            if (values.Length != 3)
            {
                Debug.LogError($"Invalid Vector3 String Value: {value}");
            }

            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }
        #endregion
    }

}