using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Audio
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _triggerSound; // Sound to play when the trigger is activated
        [SerializeField] private Dropdown _dropdown;
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                // Play the trigger sound using the AudioManager
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(_triggerSound);
                }

                Mission mission = GameManager.Instance.MissionList.Find(mission => mission.MissionTitle == "Find Trigger Box");
                // Mark the trigger box quest as complete
                if (mission != null && !mission.IsMissionCompleted())
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
