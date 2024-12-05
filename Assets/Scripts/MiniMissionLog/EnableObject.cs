using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.MiniMissionLog
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public GameObject info;

        private void Start()
        {
            info.SetActive(false);
        }

        public void WhenButtonClicked()
        {
            info.SetActive(true);
            PlayerManager.Instance.SetMissionLogUIToggle(true);
        }


    }
}
