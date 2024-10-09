using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager _instance;
        public AudioSource musicSource, sfxSource;


        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Audio Manager is Null");
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// Plays music by assigning the provided AudioClip to the music AudioSource and starting playback.
        /// This method is used to handle continuous music, such as theme songs or background loops.
        /// </summary>
        public void PlayMusic(AudioClip clip)
        {
            PlaySoundClip(musicSource, clip);
        }
        /// <summary>
        /// Plays a sound effect by assigning the provided AudioClip to the SFX AudioSource and playing it.
        /// Sound effects are generally short and are triggered by in-game events (e.g., jumping and collecting items).
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            PlaySoundClip(sfxSource, clip);

        }
        /// <summary>
        /// Plays an AudioClip on the specified AudioSource by setting the clip and starting playback.
        /// This is a utility method that abstracts the common logic of setting up and playing a sound
        /// for both music and sound effects, ensuring reuse and consistency in the way audio is handled.
        /// </summary>
        private void PlaySoundClip(AudioSource source, AudioClip clip)
        {
            /// <param name="source">The AudioSource responsible for playing the sound (music or SFX).</param>
            /// <param name="clip">The AudioClip to assign to the AudioSource for playback.</param>
            source.clip = clip;
            source.Play();
        }
    }

}


