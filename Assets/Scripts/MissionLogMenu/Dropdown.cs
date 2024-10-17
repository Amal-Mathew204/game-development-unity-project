using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripts.Game;
using Scripts.Quests;


namespace Scripts.MissonLogMenu
{
    public class Dropdown : MonoBehaviour
    {
        // Variables holding various texts for the mission log
        public TextMeshProUGUI info;
        public TMP_Dropdown dropdown;
        public TextMeshProUGUI completion;
        public TextMeshProUGUI header;
        public List<Mission> MissionList { get; private set; }

        // Boolean variables to track completion of each quest
        public bool slopeQuestComplete = false;
        public bool triggerBoxQuestComplete = false;

        private void Start()
        {
            SetHeaderVisibility(false);
            MissionList = GameManager.Instance.MissionList;
            SetMissionsInDropDown();
        }

        /// <summary>
        /// This sets the Missions in MissionsList in dropDown;
        /// </summary>
        public void SetMissionsInDropDown()
        {
            List<string> missionTitles = new List<string>();
            foreach (Mission mission in MissionList)
            {
                missionTitles.Add(mission.MissionTitle);
            }
            dropdown.AddOptions(missionTitles);
        }

        /// <summary>
        /// Handles the display of information based on the selected dropdown value.
        /// Updates the UI elements to show relevant quest information
        /// and controls the visibility of headers based on the selected value.
        /// </summary>
        public void HandleDropdownData(int option)
        {
            if (option == 0)
            {
                info.text = "";
                SetHeaderVisibility(false);
                completion.text = "";
            }
            else
            {
                Mission mission = MissionList[option - 1];
                info.text = mission.MissionInfo;
                SetHeaderVisibility(true);
                UpdateCompletionStatus(mission.IsMissionCompleted());
            }
        }

        /// <summary>
        /// Resets the mission log to its deafult state
        /// and updates the UI to reflect this change.
        /// </summary>
        public void ResetDropdown()
        {
            dropdown.value = 0;
            HandleDropdownData(0);
            SetHeaderVisibility(false);
        }

        /// <summary>
        /// Controls visibility of header and completion status
        /// </summary>
        private void SetHeaderVisibility(bool isVisible)
        {
            if (header != null)
            {
                header.gameObject.SetActive(isVisible);
            }

            if (completion != null)
            {
                completion.gameObject.SetActive(isVisible);
            }
        }

        /// <summary>
        /// Update completion status text based on the quest state
        /// </summary>
        public void UpdateCompletionStatus(bool questComplete)
        {
            if (questComplete)
            {
                completion.text = "Complete";
            }
            else
            {
                completion.text = "Incomplete";
            }
        }
    }
}