using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.MissonLogMenu
{
    public class DisableObject : MonoBehaviour
    {
        public GameObject info;

        public void WhenButtonClicked()
        {
            info.SetActive(false);
            PlayerManager.Instance.SetMissionLogUIToggle(false);
        }
    }
}
