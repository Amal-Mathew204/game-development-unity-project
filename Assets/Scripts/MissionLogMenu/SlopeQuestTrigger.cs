using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

namespace Scripts.MissonLogMenu
{
    public class SlopeQuestTrigger : MonoBehaviour
    {
        [SerializeField] private Dropdown _dropdown;

        /// <summary>
        /// Check if player has reached the top of the hill and updates the quest status
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                Mission mission = GameManager.Instance.MissionList.Find(mission => mission.MissionTitle == "Slippery Slope");
                if (mission != null && !mission.IsMissionCompleted()) // Prevent repeating the completion
                {
                    mission.SetMissionCompleted();
                    if (GameManager.Instance.MissionList.IndexOf(mission) + 1 == _dropdown.dropdown.value)
                    {
                        _dropdown.UpdateCompletionStatus(mission.IsMissionCompleted());
                    }
                }
            }
        }
    }
}
