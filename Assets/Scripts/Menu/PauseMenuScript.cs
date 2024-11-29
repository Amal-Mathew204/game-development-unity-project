using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.Menu
{
    public class PauseMenuScript : MonoBehaviour
    {
        [Header("Mission Log Components")]
        [SerializeField] private GameObject missionLogMenu;

        private void Start()
        {
            missionLogMenu = GetMissionLogMenu();
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
            GameObject gameScreen = GameObject.Find("GameScreen");
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
            missionLogMenu.SetActive(true);
        }

        /// <summary>
        /// Toggles off the mission log menu, making it invisible.
        /// </summary>
        public void CloseMissionLogMenu()
        {
            missionLogMenu.SetActive(false);
        }

    }
}

