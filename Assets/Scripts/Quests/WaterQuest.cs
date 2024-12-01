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
        /// Works as the first frame of the game
        /// Finds Mission UI interface component and assigns to variable 
        /// </summary>
        public void Start()
        {
            _dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<MissionLogDropdown>();
        }
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

