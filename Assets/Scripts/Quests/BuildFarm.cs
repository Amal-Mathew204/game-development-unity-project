using System;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Game;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.Quests
{
    public class BuildFarm : MonoBehaviour
    {
        private bool _farmBuilt = false;
        private bool _playerInTriggerBox = false;
        [SerializeField] private GameObject _farmLand;

        #region Save/Load Methods

        /// <summary>
        /// Method for saving the farmBuilt state into playerprefs as a JSON string.
        /// </summary>
        public void Save()
        {
            Dictionary<String, bool> buildFarmData = new Dictionary<String, bool>();
            buildFarmData.Add("_farmBuilt", _farmBuilt);
            PlayerPrefs.SetString("BuildFarm", JsonConvert.SerializeObject(buildFarmData));
        }

        /// <summary>
        /// Method for retrieving and deserialize farmbuild updates farmBuilt
        /// method also reactivates farmland if farm build is true
        /// </summary>
        public void Load()
        {
            string dictionary = PlayerPrefs.GetString("BuildFarm");
            Dictionary<String, bool> buildFarmData = JsonConvert.DeserializeObject<Dictionary<String, bool>>(dictionary);
            _farmBuilt = buildFarmData["_farmBuilt"];
            //If farm built was set to true reactivate farm land
            if (_farmBuilt)
            {
                _farmLand.SetActive(true);
            }
        }
        #endregion
        
        /// <summary>
        /// Method for checking if the user has entered the trigger box
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                _playerInTriggerBox = true;
                if (_farmBuilt == false && CheckShovelInInventory())
                {
                    string controls = GameSettings.Instance.UsingController ? "Button East" : "F";
                    GameScreen.Instance.ShowKeyPrompt($"Press {controls} to build the Farm");
                }
            }
        }

        /// <summary>
        /// Method for checking if the user has exited the trigger box
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                _playerInTriggerBox = false;
                if (_farmBuilt == false)
                {
                    GameScreen.Instance.HideKeyPrompt();
                }
            }
        }

        /// <summary>
        /// Method for updating mission completiton status 
        /// </summary>
        public void Update()
        {
            if (_playerInTriggerBox && CheckShovelInInventory() && PlayerManager.Instance.getTaskAccepted() && _farmBuilt == false)
            {
                _farmLand.SetActive(true);
                GameManager.Instance.SetMissionComplete("Build Farm");
                _farmBuilt = true;
                GameScreen.Instance.HideKeyPrompt();
            }
        }

        /// <summary>
        /// Method checks for shovel within inventory
        /// </summary>
        public bool CheckShovelInInventory()
        {
            if (PlayerManager.Instance == null)
            {
                return false;
            }
            List<ItemPickup> inventory = PlayerManager.Instance.Inventory;
            foreach (ItemPickup item in inventory)
            {
                if (item.itemName == "Shovel")
                {
                    return true;
                }
            }
            return false;
        }

    }
}
