using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Class Variables
        private static AudioManager _instance;
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
        #endregion

        #region Awake Methods
        private void Awake()
        {
            SetInstance();
            CheckPlayerPrefs();
            LoadPlayerPrefs();
        }
        /// <summary>
        /// Destoys any duplication
        /// </summary>
        private void SetInstance()
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

        #endregion

        #region AudioManager Methods
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
        /// Plays a SFX sound on loop 
        /// </summary>
        public void PlaySFXLoop (AudioClip clip)
        {
            PlaySoundClip(sfxSource, clip, true);
        }

        /// <summary>
        /// Stops SFX sound on loop 
        /// </summary>
        public void StopSFXLoop()
        {
            sfxSource.Stop();
        }

        /// <summary>
        /// Plays an AudioClip on the specified AudioSource by setting the clip and starting playback.
        /// Takes in a boolean value to indicate if the SFX should play in loop or not (By default its false)
        /// This is a utility method that abstracts the common logic of setting up and playing a sound
        /// for both music and sound effects, ensuring reuse and consistency in the way audio is handled.
        /// </summary>
        private void PlaySoundClip(AudioSource source, AudioClip clip,Boolean loop = false)
        {
            source.loop = loop;
            source.clip = clip;
            source.Play();
        }

        /// <summary>
        /// Sets music volume but also checks user dont go above or below limit
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            if(volume < 0 || volume > 1)
            {
                throw new ArgumentOutOfRangeException("Volume value must be between 0 to 1");
            }
            _instance.musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);

        }

        /// <summary>
        /// Sets sfx volume but also checks user dont go above or below limit
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            if (volume < 0 || volume > 1)
            {
                throw new ArgumentOutOfRangeException("Volume value must be between 0 to 1");
            }
            _instance.sfxSource.volume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
        #endregion

        #region PlayerPrefs Method
        /// <summary>
        /// Cheks player preferences
        /// </summary>
        public void CheckPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("MusicVolume") == false)
            {
                PlayerPrefs.SetFloat("MusicVolume", 1f);
            }
            if (PlayerPrefs.HasKey("SFXVolume") == false)
            {
                PlayerPrefs.SetFloat("SFXVolume", 1f);
            }
        }
        /// <summary>
        /// Load player prefernces
        /// </summary>
        public void LoadPlayerPrefs()
        {
            _instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume");
            _instance.musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
        #endregion
    }
}


