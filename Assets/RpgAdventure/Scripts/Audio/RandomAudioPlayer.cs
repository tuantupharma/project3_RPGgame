using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RpgAdventure
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        public class SoundBank
        {
            public string name;
            public AudioClip[] clips;
        }
        public bool canPlay;
        public bool isPlaying;
        public SoundBank soundbank = new SoundBank();
        private AudioSource m_Audiosource;

        private void Awake()
        {
            m_Audiosource = GetComponent<AudioSource>();
        }

        public void PlayRandomClip()
        {
            var clip = soundbank.clips[Random.Range(0, soundbank.clips.Length)];
            if (clip == null)
            {
                return;
            }

            m_Audiosource.clip = clip;
            m_Audiosource.Play();

        }

    }
    


}
