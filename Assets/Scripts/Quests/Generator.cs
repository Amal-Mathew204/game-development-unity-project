using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.GarbageDisposal;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;
using Scripts.Quests;

namespace Scripts.Quests
{
    public class Generator : MonoBehaviour
    {
        private int _count = 0;
        private MissionLogDropdown _dropdown;
        private int _fuelCellDropped = 0;
        public List<GameObject> ItemsInDisposal = new List<GameObject>();

        /// <summary>
        /// Works as the first frame of the game
        /// Finds Mission UI interface component and assigns to variable 
        /// </summary>
        private void Start()
        {
            _dropdown = GameManager.Instance.GetMissionLogDropdownComponent();

        }

        /// <summary>
        /// Updates the Place Barrel in Container Mission 
        /// </summary>
        private void Update()
        {
            if (_fuelCellDropped == 3)
            {
                ///Gets Plant Place Barrel in Container mission
                if (_dropdown == null)
                {
                    Debug.LogError("Mission UI Dropdown Component not Found");
                }
                Mission mission = _dropdown.GetMission("Turn on Generator");
                if (mission == null)
                {
                    Debug.LogError("Mission Turn on Generator Not Found");
                }
                ///Marks Mission as completed 
                mission.SetMissionCompleted();
                ///Updates Completion Status
                if (_dropdown.MissionTitles.FindIndex(title => title == mission.MissionTitle) + 1 == _dropdown.dropdown.value)
                {
                    _dropdown.UpdateCompletionStatus(true);
                }
            }
        }

        /// <summary>
        /// Method for when player enters farm trigger box with all barrels
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Destroys barrel object when dropped 
            if (other.CompareTag("FuelCell"))
            {
                _fuelCellDropped = _fuelCellDropped + 1;
            }
        }

        /// <summary>
        /// Method checks for checking if barrel is in Inventory 
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
