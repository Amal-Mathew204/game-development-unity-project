using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using Scripts.MissonLogMenu;
using Scripts.Quests;

namespace Scripts.Quests
{
    public class FallenHeroQuest : MonoBehaviour
    {



        /// <summary>
        /// Check if player has entered the trigger box. If true, set the mission to complete
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                Mission mission = GameManager.Instance.MissionList.Find(mission => mission.MissionTitle == "Fallen Hero");
                if (mission != null && !mission.IsMissionCompleted()) // Prevent repeating the completion
                {
                    mission.SetMissionCompleted();
                    Dropdown dropdown = GameManager.Instance.GetMissionLogDropdownComponent();
                    if (GameManager.Instance.MissionList.IndexOf(mission) + 1 == dropdown.dropdown.value)
                    {
                        dropdown.UpdateCompletionStatus(mission.IsMissionCompleted());
                    }
                }
            }
        }
    }
}

