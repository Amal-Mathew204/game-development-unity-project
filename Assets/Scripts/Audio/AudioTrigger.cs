using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Audio
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _triggerSound; // Sound to play when the trigger is activated
        private Dropdown _dropdownScript;

        private void Start()
        {
            // Find the Dropdown script in the scene
            _dropdownScript = FindObjectOfType<Dropdown>();

            if (_dropdownScript == null)
            {
                Debug.LogError("Dropdown script not found in the scene.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the object entering the trigger is the player
            if (other.CompareTag("Player"))
            {
                // Play the trigger sound using the AudioManager
                if (AudioManager._instance != null)
                {
                    AudioManager._instance.PlaySFX(_triggerSound);
                }

                // Mark the quest as complete if the Dropdown script is found
                if (_dropdownScript != null)
                {
                    _dropdownScript.CompleteTriggerBoxQuest();
                    _dropdownScript.UpdateCompletionStatus(_dropdownScript.triggerBoxQuestComplete);
                }
            }
        }
    }
}
