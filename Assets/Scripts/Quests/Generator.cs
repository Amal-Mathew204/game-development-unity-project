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
        private bool _missionComplete = false;
        public List<GameObject> ItemsInDisposal = new List<GameObject>();
        private AudioSource _source;

        /// <summary>
        /// Works as the first frame of the game
        /// Finds Mission UI interface component and assigns to variable 
        /// </summary>
        private void Start()
        {
            _dropdown = GameManager.Instance.GetMissionLogDropdownComponent();
            _source = GetComponent<AudioSource>();

        }

        /// <summary>
        /// Updates the Finds Fuel Cell Misison in Container Mission 
        /// </summary>
        private void Update()
        {
            if (_fuelCellDropped == 3 && _missionComplete==false)
            {
                ///Gets Finds Fuel Cell in Container mission
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
