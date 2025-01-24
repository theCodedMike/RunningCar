using UnityEngine;

namespace System.Audio.Sound.Audio
{
    public class SoundAudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private double _timeToRemove = 999.9f;
        private bool _tryToRemove;


        private void Awake()
        {
            DontDestroyOnLoad(this);
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_tryToRemove)
                TryToRemove();
        }

        public void LoadAndPlay(AudioClip newClip)
        {
            _audioSource.clip = newClip;
            _audioSource.Play();

            _timeToRemove = newClip.length;
            _tryToRemove = true;
        }

        public void TryToRemove()
        {
            _timeToRemove -= Time.deltaTime;
            if (_timeToRemove < 0)
                Destroy(gameObject);
        }
    }
}
