using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameManager = Scripts.Game.GameManager;
using PlayerManager = Scripts.Player.Player;


namespace Scripts.Player.Input
{
    public class PlayerInventoryInput : MonoBehaviour { 
        public bool toggleInventory = false;
        /// <summary>
        /// Method Toggles the visibility of the Player Inventory inside the game.
        /// The method will also toggle mouse cursor visibility dependant on the visibility of the inventory.
        /// </summary>
        public void OnToggleInventory(InputAction.CallbackContext context) {
            if (context.performed) {
                toggleInventory = !toggleInventory;
                PlayerManager.Instance.SetCursorVisibility();
            }
        }
    }
}

    

