using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.Quests
{
    public class BuildFarm : MonoBehaviour
    {
        private bool _farmBuilt = false;
        private bool _playerInTriggerBox = false;
        [SerializeField] private GameObject _farmLand;

        /// <summary>
        /// Method for checking if the user has entered the trigger box
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                _playerInTriggerBox = true;
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
            }
        }

        /// <summary>
        /// Method checks for shovel within inventory
        /// </summary>
        public bool CheckShovelInInventory()
        {
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
