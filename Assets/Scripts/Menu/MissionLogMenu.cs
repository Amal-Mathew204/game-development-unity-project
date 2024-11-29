using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using GameManager = Scripts.Game.GameManager;
using Mission = Scripts.Quests.Mission;

namespace Scripts.Menu
{
    public class MissionLogMenu : MonoBehaviour
    {
        [SerializeField] private GameObject missionCardPrefab; // Drag your Mission Card prefab here in the inspector
        [SerializeField] private Transform contentPanel; // The panel where cards will be instantiated

        void Start()
        {
            GenerateMissionCards();
        }

        private void GenerateMissionCards()
        {
            foreach (Mission mission in GameManager.Instance.MissionList)
            {
                GameObject card = Instantiate(missionCardPrefab, contentPanel);
                SetupCard(card, mission);
            }
        }

        private void SetupCard(GameObject card, Mission mission)
        {
            
        }
    }
}

