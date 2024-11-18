using UnityEngine;
using Scripts.Item;

namespace Scripts.Player
{
    public class PlayerItemRaycast : MonoBehaviour
    {
        public float raycastDistance = 1f;  
        private ItemPickup _currentItem = null;
        [SerializeField] private LayerMask _itemLayerMask;

        /// <summary>
        /// Create a Ray object starting from the player's position and going forward
        /// Define RaycastHit to store information about the object the raycast hits
        /// </summary>

        void Update()
        {

            // Create the ray going forward
            Ray forwardRay = new Ray(transform.position, transform.forward);

            // Create the ray going 45 degrees upwards
            Vector3 upwardDirection = (transform.forward + Vector3.up).normalized;
            Ray upwardRay = new Ray(transform.position, upwardDirection);

            // Create the ray going 45 degrees downwards
            
            Vector3 downwardDirection = Quaternion.Euler(25f, 0f, 90f) * Vector3.forward; // 20 degrees down
            Ray downwardRay = new Ray(transform.position, downwardDirection);

            RaycastHit hit;
            // Check if the forward ray hits something
            if (Physics.Raycast(forwardRay, out hit, raycastDistance))
            {
                Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green);  // Forward Ray in Green
                Debug.Log("Forward Ray hit: " + hit.collider.name);
            }

            // Check if the upward ray hits something
            if (Physics.Raycast(upwardRay, out hit, raycastDistance))
            {
                Debug.Log("Upward Ray hit: " + hit.collider.name);
            }

            // Check if the downward ray hits something
            if (Physics.Raycast(downwardRay, out hit, raycastDistance))
            {
                //Debug.DrawRay(transform.position, downwardDirection * 3f, Color.red);
                Debug.Log("Downward Ray hit: " + hit.collider.name);

            }
            // Perform the raycast and check if it hits something within the specified distance
            if (Physics.Raycast(forwardRay , out hit, raycastDistance, _itemLayerMask))
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

