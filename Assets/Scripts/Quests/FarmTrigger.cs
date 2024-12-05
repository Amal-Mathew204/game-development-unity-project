using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;

namespace Scripts.Quests
{
    public class FarmTrigger : MonoBehaviour
    {
        private int _count = 0;
        private bool _isSeedBagInside = false;
        [SerializeField] private GameObject _dirtPile;
        [SerializeField] private GameObject _arrowIndicator;
        [SerializeField] private SeedPlantingQuest _seedPlantingQuest;

        /// <summary>
        /// Method for when player enters farm trigger box with all seeds
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Checks if seed bag has been dropped and if there is also no other seed bags
            if (other.CompareTag("SeedBag") && _isSeedBagInside == false)
            {
                ///Destroys seed bag and actived dirtpile object
                ///Arrow indicator is also decativited and the planting quest is incremeted
                Destroy(other.gameObject);
                _isSeedBagInside = true;
                _dirtPile.SetActive(true);
                _arrowIndicator.SetActive(false);
                _seedPlantingQuest.IncrementSeedsPlanted();
            }
        }

        /// <summary>
        /// Method checks for checking if seed is in Inventory 
        /// </summary>
        public bool CheckSeedInventory()
        {
            List<ItemPickup> inventory = PlayerManager.Instance.Inventory;
            foreach (ItemPickup item in inventory)
            {
                if (item.itemName == "Seed Bag")
                {
                    _count = _count + 1;
                }
                if (_count == 4)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

