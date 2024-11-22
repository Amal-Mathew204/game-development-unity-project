using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.Menu
{
    public class PauseMenuScript : MonoBehaviour
    {
        /// <summary>
        /// Calls the Player's method to toggle the visibility of the pause menu and resume the game.
        /// </summary>
        public void ContinueGame()
        {
            PlayerManager.Instance.ChangePauseMenuVisibility();
        }
    }
}

