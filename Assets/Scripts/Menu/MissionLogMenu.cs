using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;


namespace Scripts.Menu
{
    public class MissionLogMenu : MonoBehaviour
    {
        /// <summary>
        /// toggles mission log on
        /// </summary>
        public void MissionLog()
        {
            PlayerManager.Instance.ChangePauseMenuVisibility();
            PlayerManager.Instance.ChangeMissionLogMenuVisibility();
        }
    }
}

