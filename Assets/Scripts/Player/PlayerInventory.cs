using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;  // Singleton instance

    private List<ItemPickup> inventory = new List<ItemPickup>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance exists
        }
    }

    public void AddItem(ItemPickup item)
    {
        inventory.Add(item);
        Debug.Log(item.itemName + " added to inventory.");
    }
}

