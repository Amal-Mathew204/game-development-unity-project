using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.GarbageDisposal;

namespace Scripts.Quests
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private GarbageDisposalController _garbageDisposalController;
        private int _count = 0;
        private int _oilDropped = 0;
        public List<GameObject> ItemsInDisposal = new List<GameObject>();

        /// <summary>
        /// Updates the Place Barrel in Container Mission 
        /// </summary>
        private void Update()
        {
            if (_oilDropped == 5)
            {
                 GameManager.Instance.SetMissionComplete("Place Barrels in Container");
                //set GarbageDisposalController Active
                _garbageDisposalController.isActive = true;
            }
        }

        /// <summary>
        /// Method for when player enters farm trigger box with all barrels
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Destroys barrel object when dropped 
            if (other.CompareTag("Barrel"))
            {
                _oilDropped = _oilDropped + 1;
                ItemsInDisposal.Add(other.gameObject);
            }
        }

        /// <summary>
        /// Method checks for checking if barrel is in Inventory 
        /// </summary>
        public bool CheckBarrelInventory()
        {
            List<ItemPickup> inventory = PlayerManager.Instance.Inventory;
            foreach (ItemPickup item in inventory)
            {
                if (item.itemName == "Barrel")
                {
                    _count = _count + 1;
                }
                if (_count == 5)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
