using System.Audio.Music;
using System.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.Settings
{
    public class VolumeAdjust : MonoBehaviour
    {
        [Header("音乐类型")]
        public MusicType musicType;
        [Header("Slider")]
        public Slider volumeSlider;

        public const string GameVolume = "GameVolume";
        public const string MenuVolume = "MenuVolume";
        
        
        private void Awake()
        {
            volumeSlider.onValueChanged.AddListener(AdjustVolume);
        }

        private void Start()
        {
            string volumeKey = musicType == MusicType.Game ? GameVolume : MenuVolume;
            float currVolume = CustomPlayerPrefs.GetFloat(volumeKey, 1f);
            volumeSlider.value = currVolume;
        }

        private void AdjustVolume(float volume)
        {
            string volumeKey = musicType == MusicType.Game ? GameVolume : MenuVolume;
            CustomPlayerPrefs.SetFloat(volumeKey, volume);
            MusicPlayerController.Instance.ResetVolume();
        }

        private void OnDisable()
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}
