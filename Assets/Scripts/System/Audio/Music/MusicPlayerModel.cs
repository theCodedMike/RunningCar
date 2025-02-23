using System.Utility;
using Interface.Settings;
using ScriptableObjects.Settings;
using UnityEngine;

namespace System.Audio.Music
{
    public enum MusicType
    {
        Menu, Game
    }
    public class MusicPlayerModel : MonoBehaviour
    {
        [HideInInspector]
        public AudioSource audioSource;

        public SettingsAudio settingsAudio;

        [HideInInspector]
        public AudioClip musicMenu;
        [HideInInspector]
        public AudioClip musicGame;

        public MusicType type = MusicType.Menu;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            musicMenu = settingsAudio.musicMenu;
            musicGame = settingsAudio.musicGame;
        }

        private void Start()
        {
            audioSource.volume = CustomPlayerPrefs.GetFloat(VolumeAdjust.MenuVolume, 1f);
        }

        public bool IsPlay() => CustomPlayerPrefs.GetBool("music", true);
    }
}
