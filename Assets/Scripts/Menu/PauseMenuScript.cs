using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.Menu
{
    public class PauseMenuScript : MonoBehaviour
    {
        [Header("Mission Log Components")]
        private GameObject _missionLogMenu;
        private MissionLogMenu _missionLogMenuController;


        private void Start()
        {
            _missionLogMenu = GetMissionLogMenu();
            if (_missionLogMenu == null)
            {
                Debug.LogError("Mission Log Menu is null");
            }
            _missionLogMenuController = _missionLogMenu.GetComponent<MissionLogMenu>();
            ResetMenu();
        }

        /// <summary>
        /// The Method Ensures all "pages" of the Pause Menu are inactive except for its main menu
        /// </summary>
        public void ResetMenu()
        {
            CloseMissionLogMenu();
        }
        
        /// <summary>
        /// Calls the Player's method to toggle the visibility of the pause menu and resume the game.
        /// </summary>
        public void ContinueGame()
        {
            PlayerManager.Instance.ChangePauseMenuVisibility();
        }


        /// <summary>
        /// Retrieves the Mission Log Menu GameObject by searching within the GameScreen's child objects.
        /// </summary>
        public GameObject GetMissionLogMenu()
        {
            GameObject gameScreen = GameObject.Find("Pause Menu");
            Transform[] children = gameScreen.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name == "Mission Log Menu")
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Toggles the mission log menu, making it visible.
        /// </summary>
        public void OpenMissionLogMenu()
        {
            _missionLogMenu.SetActive(true);
            _missionLogMenuController.GenerateMissionCards();

        }

        /// <summary>
        /// Toggles off the mission log menu, making it invisible.
        /// </summary>
        public void CloseMissionLogMenu()
        {
            if (_missionLogMenuController != null)
            {
                _missionLogMenuController.ClearMissionCards();
            }
            _missionLogMenu.SetActive(false);
        }
    
        /// <summary>
        /// Calls the GameManager.cs method to save the games data
        /// </summary>
        public void SaveGame()
        {
            GameManager.Instance.SaveGame();
        }
        
        /// <summary>
        /// Quits game by loading back to Start Scene
        /// </summary>
        public void QuitGame()
        {
            ContinueGame();
            GameManager.Instance.ReturnToStartMenu();
        }
    }
}

