using UnityEngine;
using TMPro;
using Scripts.Quests;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.MiniMissionLog
{
    public class DropdownButtonUpdater : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private GameObject _missionDetailPanel; 
        [SerializeField] private TextMeshProUGUI _missionDescriptionText; 
        [SerializeField] private TextMeshProUGUI _missionStatusText; 
        [SerializeField] private Transform _subMissionContainer; 
        [SerializeField] private string _defaultButtonText = "Select a Mission";

        void Start()
        {
            UpdateButtonWithPlaceholder();
            _dropdown.onValueChanged.AddListener(delegate { UpdateButtonText(); });
            ClearMissionDetails();
        }

        /// <summary>
        /// Updates the button text based on the selection of the dropdown
        /// </summary>

        private void UpdateButtonText()
        {
            if (_dropdown.value > 0 && _dropdown.value < _dropdown.options.Count)
            {
                _buttonText.text = _dropdown.options[_dropdown.value].text;
                UpdateMissionDetails(_dropdown.options[_dropdown.value].text);
            }
            else
            {
                UpdateButtonWithPlaceholder();
                ClearMissionDetails();
            }
        }

        private void UpdateMissionDetails(string missionTitle)
        {
            Mission selectedMission = FindMissionByTitle(missionTitle);
            if (selectedMission != null)
            {
                _missionDescriptionText.text = selectedMission.MissionInfo;
                // Append the item progress to the status for collect missions
                if (selectedMission is CollectMission collectMission)
                {
                    _missionDescriptionText.text += $" ({collectMission.GetItemProgress()})"; // e.g., " (3/5)"
                }
                string statusText = selectedMission.IsMissionCompleted() ? "Completed" : "Incomplete";

                

                _missionStatusText.text = statusText;
                UpdateSubMissions(selectedMission);
            }
        }



        private Mission FindMissionByTitle(string title)
        {
            // Assuming GameManager or another manager class holds all missions
            return GameManager.Instance.MissionList.Find(mission => mission.MissionTitle == title);
        }

        private void UpdateSubMissions(Mission mission)
        {
            // Clear existing sub-missions display
            foreach (Transform child in _subMissionContainer)
            {
                Destroy(child.gameObject);
            }

            // Dynamically create and configure TextMeshProUGUI elements for each sub-mission
            foreach (var subMission in mission.SubMissions)
            {
                GameObject subMissionTextObj = new GameObject("SubMissionText", typeof(TextMeshProUGUI));
                TextMeshProUGUI subMissionText = subMissionTextObj.GetComponent<TextMeshProUGUI>();

                // Configure the TextMeshPro component
                string progressText = "";
                if (subMission is CollectMission collectMission)
                {
                    progressText = $" ({collectMission.GetItemProgress()})"; // e.g., " (3/5)"
                }
                subMissionText.text = $"{subMission.MissionTitle}{progressText} - {(subMission.IsMissionCompleted() ? "C" : "X")}";

                subMissionText.fontSize = 24;
                subMissionText.alignment = TextAlignmentOptions.Left;
                subMissionText.enableWordWrapping = true;
                subMissionText.overflowMode = TextOverflowModes.Ellipsis;
                subMissionText.rectTransform.sizeDelta = new Vector2(300, 30); // Set size as needed

                // Set the parent of the sub-mission text object to the subMissionContainer
                subMissionTextObj.transform.SetParent(_subMissionContainer, false);
                subMissionTextObj.transform.localScale = Vector3.one;
            }
        }



        private void ClearMissionDetails()
        {
            _missionDescriptionText.text = "";
            _missionStatusText.text = "";
            foreach (Transform child in _subMissionContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void UpdateButtonWithPlaceholder()
        {
            _buttonText.text = _defaultButtonText;
            ClearMissionDetails();
        }
    }
}

