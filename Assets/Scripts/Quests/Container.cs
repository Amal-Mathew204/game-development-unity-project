using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.Game;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;
using Scripts.Quests;


public class Container : MonoBehaviour
{
    private int _count = 0;
    private MissionLogDropdown _dropdown;
    private int _oilDropped = 0;

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
        if (_oilDropped == 5)
        {
            ///Gets Plant Place Barrel in Container mission
            if (_dropdown == null)
            {
                Debug.LogError("Mission UI Dropdown Component not Found");
            }
            Mission mission = _dropdown.GetMission("Place Barrel in Container");
            if (mission == null)
            {
                Debug.LogError("Mission Place Barrel in Container Not Found");
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
        if (other.CompareTag("Barrel"))
        {
            _oilDropped = _oilDropped + 1;
            /////Destroys Barrel
            //Destroy(other.gameObject);
            Debug.Log("Trigger");

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