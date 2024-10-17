using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Scripts.Player.Input
{
    public class PlayerInventoryInput : MonoBehaviour { 
        public bool toggleInventory = false;
        public void OnToggleInventory(InputAction.CallbackContext context) {
            if (context.performed) {


                toggleInventory = !toggleInventory;
                Debug.Log(toggleInventory);


            }

        }
    }
}

    

