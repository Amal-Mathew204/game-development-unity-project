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
            Button closeMissionLogButton = canvas.GetChild(0).GetChild(1).gameObject.GetComponent<Button>();
            ToggleMissionLogMenu = !ToggleMissionLogMenu;
            if (ToggleMissionLogMenu)
            {
                GameManager.Instance.EnableMouseCursor();
                openMissionLogButton.onClick.Invoke();
            }
            else
            {
                GameManager.Instance.DisableMouseCursor();
                closeMissionLogButton.onClick.Invoke();
            }

        }

    }
}



