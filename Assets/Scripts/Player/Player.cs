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
    public class Player : MonoBehaviour
    {
        #region Class Variables
        public static Player Instance;  // Singleton instance
        private List<ItemPickup> _inventory = new List<ItemPickup>();
        [SerializeField] private int _maximumInventorySize = 10;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private Transform _inventoryUIParent;  // Parent UI object to hold inventory items
        private PlayerInventoryInput _playerInventoryInput;
        private List<GameObject> storedItems = new List<GameObject>();

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
            }
            else
            {
                playerInput.SwitchCurrentActionMap("Player");
            }
        }

        /// <summary>
        /// This is called to update the inventories UI, to ensure we are displaying the present version of items in the inventory
        /// </summary>
        private void UpdateInventoryUI()
        {
            if (inventoryText == null)
            {
                Debug.LogWarning("InventoryText is not assigned in the Inspector");
                return;
            }

            // Clear previous UI elements
            inventoryText.text = "";
            foreach (GameObject item in storedItems)
            {
                Destroy(item);
            }
            storedItems.Clear();

            if (_inventory.Count == 0)
            {
                inventoryText.text = "Inventory is empty";
                return;
            }

            inventoryText.text = "Inventory";
            // Create UI elements for each inventory item
            foreach (ItemPickup item in _inventory)
            {

                Object itemSlotPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ItemSlot.prefab", typeof(GameObject));
                GameObject itemSlot = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                itemSlot.transform.SetParent(_inventoryUIParent);

                TextMeshProUGUI itemSlotName = itemSlot.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI buttonName = itemSlot.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                Button itemDropButton = itemSlot.gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();

                buttonName.text = $"Drop {item.itemName}";
                itemSlotName.text = item.itemName;
                itemDropButton.onClick.AddListener(() => DropItem(item));

                storedItems.Add(itemSlot);
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
