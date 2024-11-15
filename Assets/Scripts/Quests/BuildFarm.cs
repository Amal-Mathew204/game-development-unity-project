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

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            _playerInTriggerBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            _playerInTriggerBox = false;
        }
    }

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
