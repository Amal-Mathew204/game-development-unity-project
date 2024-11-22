using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;

public class FarmTrigger : MonoBehaviour
{
    private int count = 0;
    private bool isSeedBagInside = false;
    /// <summary>
    /// Method for when player enters farm trigger box with all seeds
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CheckSeedInventory())
            {
                Debug.Log("User is allowed to start planting");
            }

        }
        if (other.CompareTag("SeedBag") && isSeedBagInside == false)
        {
            Destroy(other.gameObject);
            isSeedBagInside =  true;

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
                count = count + 1;
            }
            if (count == 5)
            {
                Debug.Log("All seeds have been found ");
                return true;
            }
        }
        return false;
    }
}
