using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Scripts.Game;
using Scripts.Item;
using Scripts.Player.Input;
using Scripts.Quests;
using Scripts.NPC;
using Ilumisoft.RadarSystem;   //this is a third party class
using UnityEditor.Rendering;


namespace Scripts.Player
{
    [DefaultExecutionOrder(-1)]
    public class Player : MonoBehaviour
    {
        #region Class Variables
        [Header("Player Components")]
        public static Player Instance;  // Singleton instance
        public PlayerQuestPointer QuestPointer;
        private PlayerInventoryInput _playerInventoryInput;
        private PlayerUIInput _playerUIInput;
        private PlayerController _playerController;
        private PlayerInput _playerInput;
        private int _mircoChips = 10;

        public int MicroChips
        {
            get
            {
                return _mircoChips;
            }
            set
            {
                this._mircoChips = value;
                //Update value on screen
                GameScreen.Instance?.UpdateMicrochipsValue(_mircoChips);
            }
        }
        [Header("Inventory Components")]
        public List<ItemPickup> Inventory = new List<ItemPickup>();
        [SerializeField] private int _maximumInventorySize = 10;
        [SerializeField] private TextMeshProUGUI _inventoryText;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private Transform _inventoryUIParent;  // Parent UI object to hold inventory items
        [SerializeField] private GameObject _inventoryWarningText;
        private bool _activateInventoryWarningMessage = false;
        private float _inventoryWarningMessageTimeDisplayed = 0f;
        private float _warningTextTimeDuration = 2f;
        private bool _isInventoryOpen = false;
        private List<GameObject> _storedItems = new List<GameObject>();

        [Header("Game Start Components")]
        private GameObject _startNPC; // Reference to the startNPC prefab
        private StartNPC _npcTrigger;
        public bool startOfGame = true;
        
        [Header("Radar Component")]
        public Radar radar;

        [Header("Pause Menu Components")]
        private GameObject _pauseMenu;
        //cache values
        private bool _pauseMenuActive = false;
        
        [Header("Player Virtual Camera Components")]
        [SerializeField] private CinemachineVirtualCamera _thirdPersonCamera;
        [SerializeField] private CinemachineVirtualCamera _firstPersonCamera;
        #endregion

        #region Awake Methods
        /// <summary>
        /// Call SetInstance on awake of this script and Set Initial Property Values
        /// </summary>
        private void Awake()
        {
            SetInstance();
            //Initialise Microchips value from Game Settings
            MicroChips = GameSettings.Instance.GameDifficultySettings.InitialMicrochips;
        }
        /// <summary>
        /// Set Single Instance of Player 
        /// </summary>
        private void SetInstance()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);  // Ensure only one instance exists
            }
        }
        #endregion

        #region Start Methods
        /// <summary>
        /// Initialise Player Components
        /// </summary>
        private void Start()
        {
            //Obtain Player Components
            _playerInventoryInput = GetComponent<PlayerInventoryInput>();
            _playerUIInput = GetComponent<PlayerUIInput>();
            _startNPC = GameObject.FindGameObjectWithTag("StartNPC");
            _npcTrigger = _startNPC.GetComponentInChildren<StartNPC>();
            _playerController = GetComponent<PlayerController>();
            _playerInput = GetComponent<PlayerInput>();
            //reference menus
            _pauseMenu = GetPauseMenu();
            //set Microchips ammount on screen
            GameScreen.Instance.UpdateMicrochipsValue(MicroChips);
        }

        /// <summary>
        /// Retrieves the Pause Menu GameObject by searching within the GameScreen's child objects.
        /// </summary>
        public GameObject GetPauseMenu()
        {
            GameObject gameScreen = GameObject.Find("GameScreen");
            Transform[] children = gameScreen.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name == "Pause Menu")
                {
                    return child.gameObject;
                }
            }
            return null;
        }
        #endregion

        #region LoadPlayerData Methods
        /// <summary>
        /// This method loads the Player Details from a Saved Game
        /// </summary>
        public void SetLoadedPlayerData(Vector3 playerPosition, int mircoChips)
        {
            SetInitialPlayerPosition(playerPosition);
            MicroChips = mircoChips;
            StartGame();
        }
        
        /// <summary>
        /// Method Sets the Initial Position of the Player (when playing a saved game)
        /// </summary>
        private void SetInitialPlayerPosition(Vector3 position)
        {
            CharacterController characterController = GetComponent<CharacterController>();
            characterController.enabled = false;
            transform.position = position;
            characterController.enabled = true;
        }
        #endregion
        
        #region Update Methods
        /// <summary>
        /// Continuously checks and updates inventory UI visibility, inventory warning message, 
        /// and pause menu visibility each frame.
        /// </summary>
        private void Update()
        {
            if (!_pauseMenuActive)
            {
                ToggleInventoryUI();
                ToggleInventoryWarningMessage();
            }
            
            TogglePauseMenu();
        }
        #endregion
        
        #region Toggle Player POV Methods
        
        /// <summary>
        /// Switches camera to first person view
        /// </summary>
        public void SwitchToFirstPerson()
        {
            _thirdPersonCamera.Priority = 5;
            _firstPersonCamera.Priority = 10;
        }
        
        /// <summary>
        /// Switches camera to third person view
        /// </summary>
        public void SwitchToThirdPerson()
        {
            _thirdPersonCamera.Priority = 10;
            _firstPersonCamera.Priority = 5;
        }
        #endregion

        #region Pause Methods
        /// <summary>
        /// Checks the input for toggling the pause menu and updates its visibility if needed.
        /// </summary>
        public void TogglePauseMenu()
        {
            if (_playerUIInput.TogglePauseMenu)
            {
                ChangePauseMenuVisibility();
            }
            
        }
        /// <summary>
        /// Method returns bool value based on if the pause menu is active on screen
        /// </summary>
        public bool IsPauseMenuActive()
        {
            return _pauseMenuActive;
        }

        /// <summary>
        /// Toggles the visibility of the pause menu, pauses/unpauses the game,
        /// and switches the current input action map between "UI" and "Player" accordingly.
        /// </summary>
        public void ChangePauseMenuVisibility()
        {
            _pauseMenuActive = !_pauseMenuActive;
            

            if (_pauseMenu != null)
            {
                _pauseMenu.SetActive(_pauseMenuActive);
                if (_pauseMenuActive)
                {
                    Time.timeScale = 0f;
                    _playerInput.SwitchCurrentActionMap("UI");
                    GameManager.Instance.EnableMouseCursor();

                }
                else
                {
                    Time.timeScale = 1f;
                    _playerInput.SwitchCurrentActionMap("Player");
                    GameManager.Instance.DisableMouseCursor();
                }
            }
            else
            {
                Debug.LogError("Player Menu Is Not Found");
            }
        }

        #endregion
        
        #region Player Inventory Methods
        /// <summary>
        /// This Method will turn on the Inventory Warning Message for three seconds if requested
        /// </summary>
        public void ToggleInventoryWarningMessage()
        {
            if (_activateInventoryWarningMessage == false)
            {
                return;
            }
            //activates field and returns
            if (_inventoryWarningMessageTimeDisplayed == 0f)
            {
                _inventoryWarningText.SetActive(true);
                _inventoryWarningMessageTimeDisplayed += Time.deltaTime;
                return;
            }
            _inventoryWarningMessageTimeDisplayed += Time.deltaTime;
            if (_inventoryWarningMessageTimeDisplayed >= _warningTextTimeDuration)
            {
                _inventoryWarningMessageTimeDisplayed = 0f;
                _activateInventoryWarningMessage = false;
                _inventoryWarningText.SetActive(false);
            }

        }

        /// <summary>
        /// Method Toggles Inventory UI visability (dependant on the users input)
        /// </summary>
        private void ToggleInventoryUI()
        {
            if (_inventoryPanel == null || _playerInventoryInput.toggleInventory  == _isInventoryOpen)
            {
                return;
            }
            _isInventoryOpen = !_isInventoryOpen;
            _inventoryPanel.SetActive(_isInventoryOpen);
            if (_isInventoryOpen)
            {
                _playerInput.SwitchCurrentActionMap("UI");
                _inventoryWarningText.SetActive(false);
                
            }
            else
            {
                _playerInput.SwitchCurrentActionMap("Player");
                if (_activateInventoryWarningMessage)
                {
                    _inventoryWarningText.SetActive(true);
                }
            }
            SetCursorVisibility();
        }

        /// <summary>
        /// This is called to update the inventories UI, to ensure we are displaying the present version of items in the inventory
        /// </summary>
        private void UpdateInventoryUI()
        {
            if (_inventoryText == null)
            {
                Debug.LogWarning("InventoryText is not assigned in the Inspector");
                return;
            }

            // Clear previous UI elements
            _inventoryText.text = "";
            foreach (GameObject item in _storedItems)
            {
                Destroy(item);
            }
            _storedItems.Clear();

            if (Inventory.Count == 0)
            {
                _inventoryText.text = "Inventory is empty";
                return;
            }

            _inventoryText.text = "Inventory";
            // Create UI elements for each inventory item
            foreach (ItemPickup item in Inventory)
            {

                GameObject itemSlotPrefab = Resources.Load("Prefabs/ItemSlot") as GameObject;
                GameObject itemSlot = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
                itemSlot.transform.SetParent(_inventoryUIParent);

                TextMeshProUGUI itemSlotName = itemSlot.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI buttonName = itemSlot.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                Button itemDropButton = itemSlot.gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();

                buttonName.text = $"Drop {item.itemName}";
                itemSlotName.text = item.itemName;
                itemDropButton.onClick.AddListener(() => DropItem(item));

                _storedItems.Add(itemSlot);
            }
        }
        #endregion

        #region Start NPC
        /// <summary>
        /// This is called at the start of the game to cycle through the starter NPC comments and enable player movement once done
        /// Once the method has finished displaying all NPC comments we disable the entertextfield and allow the player to move around
        /// in the game
        /// </summary>
        public void ContinueGame(InputAction.CallbackContext context)
        {
            if (!context.performed || startOfGame == false)
            {
                return;
            }
            

            if (_npcTrigger != null)
            {
                if(!_npcTrigger.CycleBubbleText())
                {
                    StartGame();
                }
            }
        }
        
        /// <summary>
        /// This method is used to enable movement of the player and disabling initial Text Fields Prompts
        /// Allowing the Player to continue on and start playing the game
        /// </summary>
        private void StartGame()
        {
            // Check if PlayerController exists and activate it
            PlayerController playerController = GetComponent<PlayerController>();
            PlayerInput playerInput = GetComponent<PlayerInput>();
            if (playerController != null)
            {
                playerController.enabled = true;  // Re-enable the PlayerController script
                startOfGame = false;
                playerInput.SwitchCurrentActionMap("Player");
                GameManager.Instance.ChangeEnterTextFieldVisibility(false);
            }
            else
            {
                Debug.LogError("PlayerController component not found on the player object."); 
            }
        }
        #endregion

        #region Player Item Methods
        /// <summary>
        /// Add the current item to the player's inventory
        /// </summary>
        public bool AddItem(ItemPickup item)
        {   
            if (Inventory.Count >= _maximumInventorySize)
            {
                _activateInventoryWarningMessage = true;
                return false;   
            }

            Inventory.Add(item);
            item.gameObject.SetActive(false);
            HandleItemInMission(item.itemName);
            UpdateInventoryUI();
            return true;

        }

        /// <summary>
        /// This is called to remove items from the player's inventory
        /// </summary>
        public void RemoveItem(ItemPickup item)
        {
            if (Inventory.Contains(item)) {

                Inventory.Remove(item);
                UpdateInventoryUI();
            }
        }

        /// <summary>
        /// This is the function called to instantiate items from the inventory back into the world, it also
        /// checks for if the item is a GPS Scanner, and if such deactivates the Radar UI.
        /// Checks for obstacles infront of the player, placing items behind the player if there is an object in the way
        /// </summary>
        public void DropItem(ItemPickup item)
        {
            if (item.itemName == "GPS Scanner")
            {
                radar.DeactivateRadar();
            }
            RemoveItem(item);

            Vector3 dropPosition = transform.position + transform.forward * 2f;

            int layerMask = ~LayerMask.GetMask("GarbageDisposal", "Item");

            float distance = 1.5f;
            if (item.itemName == "Dynamite")
            {
                distance = 1f;
            }
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            if (Physics.Raycast(position, transform.forward, out RaycastHit hit, distance, layerMask))
            {
                dropPosition = transform.position - transform.forward * 2f;
            }

            item.gameObject.transform.position = dropPosition;
            item.gameObject.SetActive(true);
        }
        #endregion

        #region Mission Methods
        /// <summary>
        /// Method for getting all the collect missions. Works by going through every mission and adds collect missions to the collect mission list.
        /// </summary>
        public List<CollectMission> GetCollectMissions(List<Mission> missionList)
        {
            List<CollectMission> collectMissions = new List<CollectMission>();
            foreach (Mission mission in missionList)
            {
                if (mission.GetType().Name == "CollectMission")
                {
                    CollectMission collectMission = (CollectMission)mission;
                    collectMissions.Add(collectMission);
                }
            }
            return collectMissions;
        }

        /// <summary>
        /// Method Checks if the Item Collected is involved in an active mission. If true the item is registered in the mission as collected.
        /// </summary>
        private void HandleItemInMission(string itemName)
        {
            List <CollectMission> collectMissions = new List<CollectMission>();
            foreach(Mission mission in GameManager.Instance.MissionList)
            {
                if (mission.hasSubMissions())
                {
                    collectMissions.AddRange(GetCollectMissions(mission.SubMissions));
                }
                else if(mission.GetType().Name == "CollectMission")
                {
                    CollectMission collectMission = (CollectMission)mission;
                    collectMissions.Add(collectMission);
                }
            }

            foreach (CollectMission collectMission in collectMissions)
            {
                if (collectMission == null || collectMission.IsMissionCompleted())
                {
                    continue;
                }
                collectMission.RegisterCollectedItem(itemName);
            }
        }
        #endregion

        #region MissionLog Methods
        /// <summary>
        /// Method Sets a boolean value for the class property ToggleMissionLogMenu
        /// inside the activate instance of the PlayerUIInput Class
        /// </summary>
        public void SetMissionLogUIToggle(bool isMissionLogOpen)
        {
            _playerUIInput.ToggleMissionLogMenu = isMissionLogOpen;
            SetCursorVisibility();
        }
        #endregion

        #region Cursor Methods
        /// <summary>
        /// Method Sets a boolean value for the class property ToggleMissionLogMenu
        /// inside the activate instance of the PlayerUIInput Class
        /// </summary>
        public void SetCursorVisibility()
        {
            if (_playerInventoryInput.toggleInventory || _playerUIInput.ToggleMissionLogMenu)
            {
                GameManager.Instance.EnableMouseCursor();
            }
            else
            {
                GameManager.Instance.DisableMouseCursor();
            }
        }
        #endregion
        
        #region Action Methods
        /// <summary>
        /// Method for getting all tasks that accepted
        /// </summary>
        public bool getTaskAccepted()
        {
            PlayerActionInput playerActionInput = GetComponent<PlayerActionInput>();
            return playerActionInput.IsPressingAcceptKey;
        }

        /// <summary>
        /// This method returns a boolean value dependant on if the left mouse button is held down
        /// </summary>
        public bool CheckLeftMouseButtonDown()
        {
            return _playerUIInput.MouseButtonHeldDown;
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Destroys Player GameObject
        /// </summary>
        public static void DestroyGameObject()
        {
            if (Instance == null){
                return;
            }
            Destroy(Instance.gameObject);
            Instance = null;
        }

        /// <summary>
        /// This method disables the Player Controller Script and sets the Action Map of the Player to UI
        /// </summary>
        public void DisablePlayerMovement()
        {
            _playerController.enabled = false;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        /// <summary>
        /// This method enables the Player Controller Script and sets the Action Map of the Player to UI
        /// </summary>
        public void EnablePlayerMovement()
        {
            _playerController.enabled = true;
            _playerInput.SwitchCurrentActionMap("Player");
        }

        /// <summary>
        /// The method creates a raycast from the middle of the player to determine if the Player is facing
        /// an object (determined via a raycast hist, with a given distance and layermask)
        /// </summary>
        public bool CheckPlayerIsFacingTarget(LayerMask layerMask, float raycastDistance = 3f)
        {
            CharacterController characterController = GetComponent<CharacterController>();
            Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + (characterController.height / 2), transform.position.z);
            Ray ray = new Ray(rayOrigin, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
