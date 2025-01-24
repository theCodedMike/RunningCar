using System.Audio.Sound;
using System.Audio.Sound.Audio;
using UnityEngine;

namespace System.Audio
{
    public class AudioLoader : MonoBehaviour
    {
        public AudioSource audioSource;
        public SoundType soundType;

        private void Start()
        {
            Load();
        }

        public void Load()
        {
            if (audioSource == null)
                return;

            AudioClip clip = SoundPlayerController.Instance.model.GetClip(soundType);
            audioSource.clip = clip;
        }
    }
}
