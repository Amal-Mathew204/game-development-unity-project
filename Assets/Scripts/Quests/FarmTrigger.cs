using UnityEngine;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;

public class FarmTrigger : MonoBehaviour
{
    private int count = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CheckSeedInventory())
            {
                Debug.Log("User is allowed to start planting");
            }

        }
                
    }
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
