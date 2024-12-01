using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;

namespace Scripts.Quests
{
    public class WaterQuest : MonoBehaviour
    {
        private MissionLogDropdown _dropdown;

        /// <summary>
        /// Initializes the script by finding the Mission UI component and assigning it to the dropdown variable.
        /// This is called when the script starts.
        /// </summary>
        public void Start()
        {
            _dropdown = GameManager.Instance.GetMissionLogDropdownComponent();
            
        }

        /// <summary>
        /// Detects when the player enters the trigger zone.
        /// Marks the "Water Source Location" mission as completed and updates the mission log UI accordingly.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Mission mission = _dropdown.GetMission("Water Source Location");
                
                mission.SetMissionCompleted();
                if (_dropdown.MissionTitles.FindIndex(title => title == mission.MissionTitle) + 1 == _dropdown.dropdown.value)
                {
                    _dropdown.UpdateCompletionStatus(true);
                }
            }
        }

        

    }
}

