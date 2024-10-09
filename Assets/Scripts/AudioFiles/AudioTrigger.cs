using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Audio
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _triggerSound; // Sound to play when the trigger is activated

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
            }
        }
    }
}
