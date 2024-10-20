using UnityEngine;
using Scripts.Item;

namespace Scripts.Player
{
    public class PlayerItemRaycast : MonoBehaviour
    {
        public float raycastDistance = 3f;  
        private ItemPickup _currentItem = null;


        /// <summary>
        /// Create a Ray object starting from the player's position and going forward
        /// Define RaycastHit to store information about the object the raycast hits
        /// </summary>

        void Update()
        {
        
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;  

            // Perform the raycast and check if it hits something within the specified distance
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Try to get the ItemPickup component from the object the raycast hits
                ItemPickup item = hit.collider.GetComponent<ItemPickup>();

                if (item != null)  // If we hit an item
                {
                    // If this is a new item or still the same item, handle it
                    if (item != _currentItem)
                    {
                        if (_currentItem != null)
                        {
                            // If there was a previous item, call OnRaycastExit for it
                            _currentItem.OnRaycastExit();
                        }

                        // Now assign the current item to the one we're pointing at
                        _currentItem = item;
                        _currentItem.OnRaycastHit();  
                    }

                    // Allow the player to try picking up the item if the key is pressed
                    _currentItem.TryPickUp();
                }
            }
            else if (_currentItem != null)
            {
                // If the raycast no longer hits an item, clear the _currentItem and call OnRaycastExit
                _currentItem.OnRaycastExit();
                _currentItem = null;
            }
        }
    }
}

