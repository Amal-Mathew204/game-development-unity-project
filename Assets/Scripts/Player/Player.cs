using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Item;

namespace Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;  // Singleton instance
        private List<ItemPickup> _inventory = new List<ItemPickup>();

        private void Awake()
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
        /// <summary>
        /// Add the current item to the player's inventory
        /// </summary>

        public void AddItem(ItemPickup item)
        {
            _inventory.Add(item);
            Debug.Log(item.itemName + " added to inventory.");
        }
    }
}
