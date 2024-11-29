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
            TextMeshProUGUI titleText = card.GetComponentInChildren<TextMeshProUGUI>(); // Main title

            if (titleText != null)
            {
                titleText.text = mission.MissionTitle;
            }

            // Assuming a container GameObject exists in your card prefab for sub-missions text
            Transform subMissionContainer = card.transform.Find("SubMissionContainer");
            if (subMissionContainer == null)
            {
                Debug.LogError("SubMissionContainer not found in the prefab!");
                return;
            }

            // Clear previous sub-missions texts (if any)
            foreach (Transform child in subMissionContainer)
            {
                GameObject.Destroy(child.gameObject);
            }

            // Create new TextMeshProUGUI for each sub-mission
            foreach (Mission subMission in mission.SubMissions)
            {
                GameObject subMissionTextObj = new GameObject("SubMissionText", typeof(TextMeshProUGUI));
                TextMeshProUGUI subMissionText = subMissionTextObj.GetComponent<TextMeshProUGUI>();

                // Configure the TextMeshPro component
                subMissionText.text = subMission.MissionTitle + (subMission.IsMissionCompleted() ? " (Complete)" : " (Incomplete)");
                subMissionText.fontSize = 24; // Set a smaller font size for sub-missions
                subMissionText.alignment = TextAlignmentOptions.Left;
                subMissionText.color = subMission.IsMissionCompleted() ? Color.green : Color.red; // Green for completed, red for incomplete
                subMissionText.enableWordWrapping = true;
                subMissionText.overflowMode = TextOverflowModes.Ellipsis;
                subMissionText.rectTransform.sizeDelta = new Vector2(350, 30); // Adjust width and height as needed

                // Set parent to the subMissionContainer and adjust localScale
                subMissionTextObj.transform.SetParent(subMissionContainer, false);
                subMissionTextObj.transform.localScale = Vector3.one;
            }
        }
    }
}

