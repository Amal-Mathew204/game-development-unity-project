using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;


public class BuildFarm : MonoBehaviour
{
    public bool farmBuilt = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            //TODO add extra condition here to check player is pressing F key (need to do key binding
            // for this making it mapped to a method similar to onjump)
            if (CheckShovelInInventory())
            {
                // add code to enable farm
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
