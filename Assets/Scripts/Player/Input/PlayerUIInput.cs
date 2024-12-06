using System;
using System.Collections;
using System.Collections.Generic;
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

            Transform canvas = missionUI.transform.GetChild(0);
            Button openMissionLogButton = canvas.GetChild(1).GetChild(0).gameObject.GetComponent<Button>();
            Button closeMissionLogButton = canvas.GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
            
            ToggleMissionLogMenu = !ToggleMissionLogMenu;
            if (ToggleMissionLogMenu)
            {
                openMissionLogButton.onClick.Invoke();
            }
            else
            {
                closeMissionLogButton.onClick.Invoke();
            }

        }
        /// <summary>
        /// Toggles the pause menu when the input action is performed.
        /// Sets the TogglePauseMenu flag to true when the pause input is received.
        /// </summary>
        public void OnTogglePauseMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
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



