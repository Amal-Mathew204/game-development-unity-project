using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;
using Scripts.Quests;


public class Container : MonoBehaviour
{
    private int _count = 0;
    [SerializeField] private SeedPlantingQuest _seedPlantingQuest;
    /// <summary>
    /// Method for when player enters farm trigger box with all barrels
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        ///Mission mission = _dropdown.GetMission("Plant Seed");
        ///Destroys barrel object when dropped 
        if (other.CompareTag("Barrel"))
        {
            ///Destroys Barrel
            Destroy(other.gameObject);
        }


    }
    /// <summary>
    /// Method checks for checking if barrel is in Inventory 
    /// </summary>
    public bool CheckSeedInventory()
    {
        List<ItemPickup> inventory = PlayerManager.Instance.Inventory;
        foreach (ItemPickup item in inventory)
        {
            if (item.itemName == "Barrel")
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