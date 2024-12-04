using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager = Scripts.Game.GameManager;
using Mission = Scripts.Quests.Mission;
using CollectMission = Scripts.Quests.CollectMission;
using TMPro;

namespace Scripts.Menu
{
    public class MissionLogMenu : MonoBehaviour
    {
        [SerializeField] private GameObject missionCardPrefab; 
        [SerializeField] private Transform contentPanel; // The panel where cards will be instantiated
        public List<Mission> MissionList { get; private set; }
        public List<string> MissionTitles { get; private set; }
        public List<Mission> SubMissions { get; private set; } = new List<Mission>();
        [SerializeField] private TMP_Dropdown _dropdown;

        void Start()
        {
            MissionList = GameManager.Instance.MissionList;
            PopulateDropdown();
        }

        /// <summary>
        /// Populates the dropdown menu dynamically with mission titles
        /// </summary>
        void PopulateDropdown()
        {
            _dropdown.ClearOptions();  // Clear existing options
            List<string> missionTitles = new List<string>();

            foreach (var mission in GameManager.Instance.MissionList)
            {
                missionTitles.Add(mission.MissionTitle);
            }

            _dropdown.AddOptions(missionTitles);  
        }

        /// <summary>
        /// Generates a card for each mission in the game manager's mission list
        /// </summary>
        public void GenerateMissionCards()
        {
            foreach (Mission mission in GameManager.Instance.MissionList)
            {
                GameObject card = Instantiate(missionCardPrefab, contentPanel);
                SetupCard(card, mission); 
            }
        }

        /// <summary>
        /// Configures the cards for each mission with all its relvant information
        /// Including title, completion status, description, submissions (if any) and their completion status, and a progress counter (where relevant)
        /// </summary>
        private void SetupCard(GameObject card, Mission mission)
        {
            TextMeshProUGUI titleText = card.GetComponentInChildren<TextMeshProUGUI>(); // Mission title
            TextMeshProUGUI descriptionText = card.transform.Find("Description").GetComponent<TextMeshProUGUI>(); // Misstion description
            TextMeshProUGUI itemProgressText = card.transform.Find("Progress").GetComponent<TextMeshProUGUI>(); // Progress counter for collect missions
            Transform subMissionContainer = card.transform.Find("SubMissionContainer"); // Submission container


            // Display title
            if (titleText != null)
            {
                titleText.text = mission.MissionTitle + (mission.IsMissionCompleted() ? " (Complete)" : " (Incomplete)");
            }

            // Display description
            if (descriptionText != null)
            {
                descriptionText.text = mission.MissionInfo; // Set the mission description text
            }
           
            // Check for submission container in the prefab
            if (subMissionContainer == null)
            {
                Debug.LogError("SubMissionContainer not found in the prefab!");
                return;
            }
            
            // Set item progress if it's a CollectMission
            if (mission is CollectMission collect)
            {
                itemProgressText.gameObject.SetActive(true);
                itemProgressText.text = collect.GetItemProgress();
            }
            else
            {
                itemProgressText.gameObject.SetActive(false); 
            }

            // Only display sub-missions if they exist
            if (mission.hasSubMissions())
            {
                subMissionContainer.gameObject.SetActive(true);
                foreach (Mission subMission in mission.SubMissions)
                {
                    GameObject subMissionTextObj = new GameObject("SubMissionText", typeof(TextMeshProUGUI));
                    TextMeshProUGUI subMissionText = subMissionTextObj.GetComponent<TextMeshProUGUI>();

                    // Update text to include item progress if it's a CollectMission
                    string progressText = "";
                    if (subMission is CollectMission collectMission)
                    {
                        progressText = collectMission.GetItemProgress() + " ";
                    }

                    // All the information and format of submissions
                    subMissionText.text = subMission.MissionTitle + " " + progressText + (subMission.IsMissionCompleted() ? " (Complete)" : " (Incomplete)");
                    subMissionText.fontSize = 24;
                    subMissionText.alignment = TextAlignmentOptions.Left;
                    subMissionText.enableWordWrapping = true;
                    subMissionText.overflowMode = TextOverflowModes.Ellipsis;
                    subMissionText.rectTransform.sizeDelta = new Vector2(350, 30);

                    // Set the parent of the sub-mission text object to the subMissionContainer
                    subMissionTextObj.transform.SetParent(subMissionContainer, false);
                    subMissionTextObj.transform.localScale = Vector3.one;
                }
            }
            else
            {
                subMissionContainer.gameObject.SetActive(false); // Hide if no sub-missions
            }
        }

        /// <summary>
        /// Clears all the mission cards from the panel
        /// Providing empty panel for next generation (each time mission log is opened)
        /// </summary>
        public void ClearMissionCards()
        {
            foreach (Transform child in contentPanel)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}

