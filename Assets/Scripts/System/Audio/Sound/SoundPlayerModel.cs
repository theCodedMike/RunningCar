using System.Audio.Sound.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Utility;
using ScriptableObjects.Settings;
using UnityEngine;

namespace System.Audio.Sound
{
    public class SoundPlayerModel : MonoBehaviour
    {
        public GameObject soundAudioPlayer;
        public SettingsAudio settingsAudio;
        [HideInInspector]
        public List<SoundAudioClip> clips;

        public bool IsPlay() => CustomPlayerPrefs.GetBool("sound", true);

        public AudioClip GetClip(SoundType audioType)
        {
            return (from clip in clips where clip.audioType == audioType select clip.audioClip).FirstOrDefault();
        }
    }
}
