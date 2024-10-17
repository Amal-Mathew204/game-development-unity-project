using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Game;
using Scripts.Item;
using Scripts.MissonLogMenu;
using Scripts.Quests;

namespace Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;  // Singleton instance
        private List<ItemPickup> _inventory = new List<ItemPickup>();

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

        #region Player Item System Methods
        /// <summary>
        /// Add the current item to the player's inventory
        /// </summary>
        public void AddItem(ItemPickup item)
        {
            _inventory.Add(item);
            Debug.Log(item.itemName + " added to inventory.");
            HandleItemInMission(item.itemName);
        }

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
