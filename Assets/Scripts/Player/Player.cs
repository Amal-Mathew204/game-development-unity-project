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
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;



namespace Scripts.Player
{
    [DefaultExecutionOrder(-1)]
    public class Player : MonoBehaviour
    {
        #region Class Variables
        public static Player Instance;  // Singleton instance
        private List<ItemPickup> _inventory = new List<ItemPickup>();
        [SerializeField] private int _maximumInventorySize = 10;
        [SerializeField] private TextMeshProUGUI _inventoryText;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private Transform _inventoryUIParent;  // Parent UI object to hold inventory items
        [SerializeField] private GameObject _inventoryWarningText;
        private bool _activateInventoryWarningMessage = false;
        private float _inventoryWarningMessageTimeDisplayed = 0f;
        private float _warningTextTimeDuration = 2f;
        private PlayerInventoryInput _playerInventoryInput;
        private List<GameObject> _storedItems = new List<GameObject>();

        //cache values
        private bool _isInventoryOpen = false;
        #endregion

        #region Awake Methods
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
        /// Initalise Player Inventory Input Private Field
        /// </summary>
        private void Start()
        {
            _playerInventoryInput = GetComponent<PlayerInventoryInput>();
        }
        #endregion

        #region Update Methods
        private void Update()
        {
            ToggleInventoryUI();
            ToggleInventoryWarningMessage();
        }

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
            if(_inventoryWarningMessageTimeDisplayed == 0f)
            {
                _inventoryWarningText.SetActive(true);
                _inventoryWarningMessageTimeDisplayed += Time.deltaTime;
                return;
            }
            _inventoryWarningMessageTimeDisplayed += Time.deltaTime;
            if(_inventoryWarningMessageTimeDisplayed >= _warningTextTimeDuration)
            {
                _inventoryWarningMessageTimeDisplayed = 0f;
                _activateInventoryWarningMessage = false;
                _inventoryWarningText.SetActive(false);
            }

        }
        #endregion

        #region Player Inventory Methods
        /// <summary>
        /// Method Toggles Inventory UI visability (dependant on the users input)
        /// </summary>
        private void ToggleInventoryUI()
        {
            if (_inventoryPanel == null || _playerInventoryInput.toggleInventory == _isInventoryOpen)
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
