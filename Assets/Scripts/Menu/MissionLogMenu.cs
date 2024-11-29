using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using GameManager = Scripts.Game.GameManager;
using Mission = Scripts.Quests.Mission;
using TMPro;

namespace Scripts.Menu
{
    public class MissionLogMenu : MonoBehaviour
    {
        [SerializeField] private GameObject missionCardPrefab; // Drag your Mission Card prefab here in the inspector
        [SerializeField] private Transform contentPanel; // The panel where cards will be instantiated
        public List<Mission> MissionList { get; private set; }
        public List<string> MissionTitles { get; private set; }
        public List<Mission> SubMissions { get; private set; } = new List<Mission>();


        void Start()
        {
            GenerateMissionCards();
            MissionList = GameManager.Instance.MissionList;

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
            TextMeshProUGUI titleText = card.GetComponentInChildren<TextMeshProUGUI>(); // Find the TextMeshPro component
            if (titleText != null)
            {
                titleText.text = mission.MissionTitle; // Set the text to the mission's title
                Debug.Log("Assigned Mission Title: " + mission.MissionTitle); // Debug to confirm it's working
            }
            else
            {
                Debug.LogError("TextMeshPro component not found on the mission card prefab!");
            }
        }
    }
}

