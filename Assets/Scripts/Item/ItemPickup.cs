using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;  
    private bool isInRange = false;  // This will be true when the player is looking at the item

    /// <summary>
    /// This method will be called when the raycast hits this object
    /// </summary>
    public void OnRaycastHit()
    {
        Debug.Log("Press E to pick up " + itemName);
        isInRange = true;  
    }

    /// <summary>
    /// This method will be called when the raycast no longer hits this object
    /// </summary>
    public void OnRaycastExit()
    {
        Debug.Log("Out of range of " + itemName);
        isInRange = false;
    }
    /// <summary>
    /// Method to try picking up the item if the player is in range and presses the interact button
    /// </summary>
    public void TryPickUp()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.Instance.AddItem(this);  
            Debug.Log(itemName + " picked up!");
            Destroy(gameObject); 
        }
    }
}
