using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.MissonLogMenu;
using Scripts.Quests;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;

public class BuildFarm : MonoBehaviour
{
    private bool _farmBuilt = false;
    [SerializeField] private Dropdown _dropdown;
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
            Mission mission = _dropdown.GetMission("Build Farm");
            _farmLand.SetActive(true);
            mission.SetMissionCompleted();
            MissionLogDropdown dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<MissionLogDropdown>();
            if (dropdown.MissionTitles.FindIndex(title => title == mission.MissionTitle) + 1 == dropdown.dropdown.value)
            {
                dropdown.UpdateCompletionStatus(true);
            }
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
