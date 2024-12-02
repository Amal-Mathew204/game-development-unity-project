using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            MissionList = GameManager.Instance.MissionList;

        }


        public void GenerateMissionCards()
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
                titleText.text = mission.MissionTitle + (mission.IsMissionCompleted() ? " (Complete)" : " (Incomplete)");
            }

            // Get the description text component and update it
            TextMeshProUGUI descriptionText = card.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            if (descriptionText != null)
            {
                descriptionText.text = mission.MissionInfo; // Set the mission description text
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

            // Only populate sub-missions if they exist
            if (mission.hasSubMissions())
            {
                subMissionContainer.gameObject.SetActive(true);
                foreach (Mission subMission in mission.SubMissions)
                {
                    GameObject subMissionTextObj = new GameObject("SubMissionText", typeof(TextMeshProUGUI));
                    TextMeshProUGUI subMissionText = subMissionTextObj.GetComponent<TextMeshProUGUI>();

                    subMissionText.text = subMission.MissionTitle + (subMission.IsMissionCompleted() ? " (Complete)" : " (Incomplete)");
                    subMissionText.fontSize = 24;
                    subMissionText.alignment = TextAlignmentOptions.Left;
                    subMissionText.color = subMission.IsMissionCompleted() ? Color.green : Color.red;
                    subMissionText.enableWordWrapping = true;
                    subMissionText.overflowMode = TextOverflowModes.Ellipsis;
                    subMissionText.rectTransform.sizeDelta = new Vector2(350, 30);

                    subMissionTextObj.transform.SetParent(subMissionContainer, false);
                    subMissionTextObj.transform.localScale = Vector3.one;
                }
            }
            else
            {
                subMissionContainer.gameObject.SetActive(false); // Hide if no sub-missions
            }
        }

        public void ClearMissionCards()
        {
            foreach (Transform child in contentPanel)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}

