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


        /// <summary>
        /// Initializes the UI components when the script instance is first loaded
        /// </summary>
        void Start()
        {
            UpdateButtonWithPlaceholder();
            _dropdown.onValueChanged.AddListener(delegate { UpdateButtonText(); });
            ClearMissionDetails();
        }

        /// <summary>
        /// Ensures that the UI updates appropriately in response to changes in mission progress and completion status
        /// </summary>
        void OnEnable()
        {
            CollectMission.OnMissionUpdated += RefreshMissionDisplay;
            CollectMission.OnMissionStatusUpdated += UpdateCompletionStatus;
        }

        /// <summary>
        /// Unsubscribes from mission-related events when the panel becomes inactive
        /// </summary>
        void OnDisable()
        {
            CollectMission.OnMissionUpdated -= RefreshMissionDisplay;
            CollectMission.OnMissionStatusUpdated -= UpdateCompletionStatus;
        }

        /// <summary>
        /// Checks if there is a valid selection in the dropdown and then updates the mission details accordingly
        /// </summary>
        private void RefreshMissionDisplay()
        {
            if (_dropdown.options.Count > 0 && _dropdown.value > 0)
            {
                UpdateMissionDetails(_dropdown.options[_dropdown.value].text);
            }
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

        /// <summary>
        /// Updates the mission details in the UI based on the selected mission title
        /// </summary>
        private void UpdateMissionDetails(string missionTitle)
        {
            Mission selectedMission = FindMissionByTitle(missionTitle);
            if (selectedMission != null)
            {
                _missionDescriptionText.text = selectedMission.MissionInfo;

                if (selectedMission is CollectMission collectMission)
                {
                    _missionDescriptionText.text += $" ({collectMission.GetItemProgress()})"; // Displays counter in fraction format
                }

                string statusText = selectedMission.IsMissionCompleted() ? "Complete" : "Incomplete";
                _missionStatusText.text = statusText;
                UpdateSubMissions(selectedMission);
            }
        }

        /// <summary>
        /// Retrieves a mission by its title from the list of missions managed by the GameManager
        /// </summary>
        private Mission FindMissionByTitle(string title)
        {
            return GameManager.Instance.MissionList.Find(mission => mission.MissionTitle == title);
        }

        /// <summary>
        /// Updates the UI display for sub-missions related to a selected main mission
        /// </summary>
        private void UpdateSubMissions(Mission mission)
        {
            // Clear existing sub-missions display
            foreach (Transform child in _subMissionContainer)
            {
                Destroy(child.gameObject);
            }

            // Dynamically creates elements for submissions
            foreach (var subMission in mission.SubMissions)
            {
                GameObject subMissionTextObj = new GameObject("SubMissionText", typeof(TextMeshProUGUI));
                TextMeshProUGUI subMissionText = subMissionTextObj.GetComponent<TextMeshProUGUI>();

                // Retrieves completion status for each submission
                string progressText = "";
                if (subMission is CollectMission collectMission)
                {
                    progressText = $" ({collectMission.GetItemProgress()})"; 
                }

                subMissionText.text = $"{subMission.MissionTitle}{progressText} - {(subMission.IsMissionCompleted() ? "C" : "X")}"; // Marking "Complete" as "C" and "Incomplete" as "X"

                subMissionText.fontSize = 24;
                subMissionText.alignment = TextAlignmentOptions.Left;
                subMissionText.enableWordWrapping = true;
                subMissionText.overflowMode = TextOverflowModes.Ellipsis;
                subMissionText.rectTransform.sizeDelta = new Vector2(300, 30); 

                subMissionTextObj.transform.SetParent(_subMissionContainer, false);
                subMissionTextObj.transform.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Clears all mission-related information from the UI
        /// </summary>
        private void ClearMissionDetails()
        {
            _missionDescriptionText.text = "";
            _missionStatusText.text = "";
            foreach (Transform child in _subMissionContainer)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Sets the button text to a default placeholder and clears any existing mission details
        /// </summary>
        private void UpdateButtonWithPlaceholder()
        {
            _buttonText.text = _defaultButtonText;
            ClearMissionDetails();
        }

        /// <summary>
        /// Updates the display of the mission's completion status based on the current selection in the dropdown menu
        /// </summary>
        private void UpdateCompletionStatus()
        {
            if (_dropdown.options.Count > 0 && _dropdown.value > 0)
            {
                UpdateMissionDetails(_dropdown.options[_dropdown.value].text);  
            }
        }

    }
}

