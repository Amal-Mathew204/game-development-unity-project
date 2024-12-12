using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.GarbageDisposal;
using Scripts.Quests;

namespace Scripts.Quests
{
    public class Generator : MonoBehaviour
    {
        private int _count = 0;
        private int _fuelCellDropped = 0;
        private bool _missionComplete = false;
        public List<GameObject> ItemsInDisposal = new List<GameObject>();
        private AudioSource _source;

        /// <summary>
        /// Works as the first frame of the game
        /// Finds Mission UI interface component and assigns to variable 
        /// </summary>
        private void Start()
        {
            _source = GetComponent<AudioSource>();

        }

        /// <summary>
        /// Updates the Finds Fuel Cell Misison in Container Mission 
        /// </summary>
        private void Update()
        {
            if (_fuelCellDropped == 3 && _missionComplete == false)
            {
                GameManager.Instance.SetMissionComplete("Turn on Generator");
                _missionComplete = true;
            }
        }

        /// <summary>
        /// Method for when player enters farm trigger box with all Fuel Cell
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Destroys Fuel Cell object when dropped 
            if (other.CompareTag("FuelCell"))
            {
                _fuelCellDropped = _fuelCellDropped + 1;
                _source.Play();
                Destroy(other.gameObject);
            }
        }

        /// <summary>
        /// Method checks for checking if Fuel Cell is in Inventory 
        /// </summary>
        public bool CheckFuellCellInventory()
        {
            List<ItemPickup> inventory = PlayerManager.Instance.Inventory;
            foreach (ItemPickup item in inventory)
            {
                if (item.itemName == "Fuel Cell")
                {
                    _count = _count + 1;
                }
                if (_count == 3)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
