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

        public void PlayMusic(AudioClip clip)
        {
            PlaySoundClip(musicSource, clip);
        }

        public void PlaySFX(AudioClip clip)
        {
            PlaySoundClip(sfxSource, clip);

        }

        private void PlaySoundClip(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

    }

}


