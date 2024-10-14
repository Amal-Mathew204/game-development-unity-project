using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public float raycastDistance = 3f;  
    private ItemPickup currentItem = null;  

    void Update()
    {
        // Create a Ray object starting from the player's position and going forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;  // Define RaycastHit to store information about the object the raycast hits

        // Perform the raycast and check if it hits something within the specified distance
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Try to get the ItemPickup component from the object the raycast hits
            ItemPickup item = hit.collider.GetComponent<ItemPickup>();

            if (item != null)  // If we hit an item
            {
                // If this is a new item or still the same item, handle it
                if (item != currentItem)
                {
                    if (currentItem != null)
                    {
                        // If there was a previous item, call OnRaycastExit for it
                        currentItem.OnRaycastExit();
                    }

                    // Now assign the current item to the one we're pointing at
                    currentItem = item;
                    currentItem.OnRaycastHit();  
                }

                // Allow the player to try picking up the item if the key is pressed
                currentItem.TryPickUp();
            }
        }
        else if (currentItem != null)
        {
            // If the raycast no longer hits an item, clear the currentItem and call OnRaycastExit
            currentItem.OnRaycastExit();
            currentItem = null;
        }
    }
}
