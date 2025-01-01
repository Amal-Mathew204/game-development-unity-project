using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.MiniMissionLog;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameManager = Scripts.Game.GameManager;


namespace Scripts.Player.Input
{
    public class PlayerUIInput : MonoBehaviour
    {
        public bool ToggleMissionLogMenu { get; set; } = false;
        public bool TogglePauseMenu { get; set; } = false;
        public bool MouseButtonHeldDown { get; set; } = false;
        
        
        /// <summary>
        /// Method Toggles the visibility of the Mission Log Menu inside the game.
        /// The method will also toggle mouse cursor visibility dependant on the visibility of the inventory.
        /// </summary>
        public void OnToggleMissionLogMenu(InputAction.CallbackContext context)
        {
            if (!context.performed || SceneManager.GetActiveScene().name != "GameScene")
            {
                return;
            }
            GameObject missionUI = GameObject.Find("MissionUI");
            if (missionUI == null)
            {
                Debug.LogError("Mission UI Does Not Exist");
                return;
            }
            MiniMissionLogController controller = missionUI.GetComponent<MiniMissionLogController>();
            ToggleMissionLogMenu = !ToggleMissionLogMenu;
            
            if (ToggleMissionLogMenu)
            {
                controller.OpenMissionLog();
            }
            else
            {
                controller.CloseMissionLog();
            }

        }
        /// <summary>
        /// Toggles the pause menu when the input action is performed.
        /// Sets the TogglePauseMenu flag to true when the pause input is received.
        /// Pause Menu can only be toggled if the game has not ended
        /// </summary>
        public void OnTogglePauseMenu(InputAction.CallbackContext context)
        {
            if (context.performed && GameManager.Instance.HasGameEnded == false)
            {
                TogglePauseMenu = true;
            }
        }
        
        /// <summary>
        /// This method toggles whether the mouse button is being held down
        /// </summary>
        public void GetMouseButtonDown(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MouseButtonHeldDown = true;
            }

            if (context.canceled)
            {
                MouseButtonHeldDown = false;
            }
        }

        /// <summary>
        /// Resets the TogglePauseMenu to false after each frame, ensuring it's only triggered once per input
        /// Ensures that the pause menu does not flicker on and off
        /// </summary>
        public void LateUpdate()
        {
            TogglePauseMenu = false;
        }

    }
}



