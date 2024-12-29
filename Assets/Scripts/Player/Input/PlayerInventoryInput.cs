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
        /// This method is not allowed to toggle the inventory or cursor visibility until the start of game, steps are completed
        /// </summary>
        public void OnToggleInventory(InputAction.CallbackContext context) {
            if (context.performed && PlayerManager.Instance.startOfGame == false 
                && PlayerManager.Instance.IsPauseMenuActive() == false) 
            {
                toggleInventory = !toggleInventory;
                PlayerManager.Instance.SetCursorVisibility();
            }
        }
    }
}

    

