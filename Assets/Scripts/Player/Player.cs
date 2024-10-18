using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Game;
using Scripts.Item;
using Scripts.MissonLogMenu;
using Scripts.Player.Input;
using Scripts.Quests;
using TMPro;
using UnityEngine.InputSystem;


namespace Scripts.Player
{
    public class Player : MonoBehaviour
    {
        #region Class Variables
        public static Player Instance;  // Singleton instance
        private List<ItemPickup> _inventory = new List<ItemPickup>();
        private const int _maximumInventorySize = 10;
        [SerializeField] private TextMeshProUGUI inventoryText;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private GameObject _dropButtonPrefab;
        [SerializeField] private Transform _inventoryUIParent;  // Parent UI object to hold inventory items
        private PlayerInventoryInput _playerInventoryInput;
        private List<GameObject> activeButtons = new List<GameObject>();

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
        /// 
        /// </summary>
        private void UpdateInventoryUI()
        {
            if (inventoryText == null)
            {
                Debug.LogWarning("InventoryText is not assigned in the Inspector");
                return;
            }

            inventoryText.text = "";
            foreach (GameObject button in activeButtons)
            {
                Destroy(button);
            }
            activeButtons.Clear();

            if (_inventory.Count == 0)
            {
                inventoryText.text = "Inventory is empty";
                return;
            }

            string itemList = "Inventory: \n";
            foreach (ItemPickup item in _inventory)
            {
                itemList += "\t" + item.name + "\n";

                //inventoryText.text += item.itemName + "\n";

                GameObject dropButton = Instantiate(_dropButtonPrefab, _inventoryUIParent);
                dropButton.GetComponentInChildren<TextMeshProUGUI>().text = "Drop" + item.itemName + " \n";
                dropButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => DropItem(item));
                activeButtons.Add(dropButton);
            }

            inventoryText.text = itemList;
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
                Debug.Log(_inventory.Count);
                Debug.Log("Cannot add " + item.itemName + " to inventory. Inventory is full.");
                return false;
                
            }
            else
            {
                _inventory.Add(item);
                Debug.Log(item.itemName + " added to inventory.");
            }

            Debug.Log(_inventory.Count);
            HandleItemInMission(item.itemName);
            UpdateInventoryUI();
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveItem(ItemPickup item)
        {
            if (_inventory.Contains(item)) {

                _inventory.Remove(item);
                Debug.Log(item.itemName + " removed from inventory.");
                UpdateInventoryUI();
            }
            else {

                Debug.Log("Item not found in inventory.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DropItem(ItemPickup item)
        {
            RemoveItem(item);

            Vector3 dropPosition = transform.position + transform.forward * 2f;
            GameObject droppedItem = Instantiate(item.gameObject, dropPosition, Quaternion.identity);
            droppedItem.SetActive(true);

            Destroy(item);

            Debug.Log(item.itemName + " dropped into the world.");  
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
            Dropdown dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<Dropdown>();
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
