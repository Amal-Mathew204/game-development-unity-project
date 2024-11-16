using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Scripts.Game;
using Scripts.Item;
using Scripts.MissonLogMenu;
using Scripts.Player.Input;
using Scripts.Quests;
using Scripts.NPC;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;
using Unity.VisualScripting;




namespace Scripts.Player
{
    [DefaultExecutionOrder(-1)]
    public class Player : MonoBehaviour
    {
        #region Class Variables
        [Header("Player Components")]
        public static Player Instance;  // Singleton instance
        private PlayerInventoryInput _playerInventoryInput;
        private PlayerUIInput _playerUIInput;


        [Header("Inventory Components")]
        private List<ItemPickup> _inventory = new List<ItemPickup>();
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


        //cache values

        #endregion

        #region Awake Methods
        /// <summary>
        /// Call SetInstance on awake of this script
        /// </summary>
        private void Awake()
        {
            SetInstance();
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
        /// Initalise Player Inventory Input Private Field, the starter NPC and the NPCtrigger
        /// </summary>
        private void Start()
        {
            _playerInventoryInput = GetComponent<PlayerInventoryInput>();
            _playerUIInput = GetComponent<PlayerUIInput>();
            _startNPC = GameObject.FindGameObjectWithTag("StartNPC");
            _npcTrigger = _startNPC.GetComponentInChildren<StartNPC>();
            
        }
        #endregion

        #region Update Methods
        private void Update()
        {
            
            ToggleInventoryUI();
            ToggleInventoryWarningMessage();
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
            PlayerInput playerInput = GetComponent<PlayerInput>();
            if (_isInventoryOpen)
            {
                playerInput.SwitchCurrentActionMap("UI");
                _inventoryWarningText.SetActive(false);
            }
            else
            {
                playerInput.SwitchCurrentActionMap("Player");
                if (_activateInventoryWarningMessage)
                {
                    _inventoryWarningText.SetActive(true);
                }
            }
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

            if (_inventory.Count == 0)
            {
                _inventoryText.text = "Inventory is empty";
                return;
            }

            _inventoryText.text = "Inventory";
            // Create UI elements for each inventory item
            foreach (ItemPickup item in _inventory)
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
            if (!context.performed)
            {
                return;
            }
            Debug.Log("Game Continued");

            if (_npcTrigger != null)
            {
                if(!_npcTrigger.CycleBubbleText())
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
            }
        }
        #endregion

        #region Player Item Methods
        /// <summary>
        /// Add the current item to the player's inventory
        /// </summary>
        public bool AddItem(ItemPickup item)
        {   
            if (_inventory.Count >= _maximumInventorySize)
            {
                _activateInventoryWarningMessage = true;
                return false;   
            }

            _inventory.Add(item);
            HandleItemInMission(item.itemName);
            UpdateInventoryUI();
            return true;

        }

        /// <summary>
        /// This is called to remove items from the player's inventory
        /// </summary>
        public void RemoveItem(ItemPickup item)
        {
            if (_inventory.Contains(item)) {

                _inventory.Remove(item);
                UpdateInventoryUI();
            }
        }

        /// <summary>
        /// This is the function called to instantiate items from the inventory back into the world
        /// </summary>
        public void DropItem(ItemPickup item)
        {
            RemoveItem(item);

            Vector3 dropPosition = transform.position + transform.forward * 2f;
            GameObject droppedItem = Instantiate(item.gameObject, dropPosition, Quaternion.identity);
            droppedItem.SetActive(true);

            Destroy(item);
        }
        #endregion

        #region Mission Methods
        /// <summary>
        /// Method Checks if the Item Collected is involved in an active mission. If true the item is registered in the mission as collected.
        /// </summary>
        private void HandleItemInMission(string itemName)
        {
            Mission mission = GameManager.Instance.MissionList.Find(mission => mission.GetType().Name == "CollectMission");
            if(mission == null || mission.IsMissionCompleted())
            {
                return;
            }
            //We can type cast since we are sure its a collect mission object
            CollectMission collectMission = (CollectMission)mission;
            collectMission.RegisterCollectedItem(itemName);

            if (collectMission.IsMissionCompleted())
            {
                UpdateDropDownMissionStatus(collectMission);
            }
        }

        /// <summary>
        /// Method Checks If Mission is selected in dropdown menu. If true the mission status is updated on the dropdown menu.
        /// </summary>
        private void UpdateDropDownMissionStatus(CollectMission collectMission)
        {
            MissionLogDropdown dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<MissionLogDropdown>();
            if (GameManager.Instance.MissionList.IndexOf(collectMission) + 1 == dropdown.dropdown.value)
            {
                dropdown.UpdateCompletionStatus(true);
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
        #endregion
    }
}
