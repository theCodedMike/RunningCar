using System.Audio.Sound.Audio;
using System.Collections;
using ScriptableObjects.Settings;
using UnityEngine;

namespace System.Audio.Sound
{
    [RequireComponent(typeof(SoundPlayerModel))]
    public class SoundPlayerController : MonoBehaviour
    {
        public static SoundPlayerController Instance { get; private set; }

        [HideInInspector]
        public SoundPlayerModel model;


        #region Standard system methods
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;

                model = GetComponent<SoundPlayerModel>();
                LoadClips();
            } else 
                Destroy(gameObject);
        }
        #endregion


        public void LoadClips()
        {
            SettingsAudio resource = model.settingsAudio;
            model.clips = new(16);
            
            model.clips.Add(new SoundAudioClip(resource.audioButtonClick, SoundType.ButtonClick));
            model.clips.Add(new SoundAudioClip(resource.audioSwitch, SoundType.Switch));
            model.clips.Add(new SoundAudioClip(resource.audioError, SoundType.Error));
            model.clips.Add(new SoundAudioClip(resource.audioSuccessful, SoundType.Successful));
            model.clips.Add(new SoundAudioClip(resource.audioEquip, SoundType.Equip));
            
            model.clips.Add(new SoundAudioClip(resource.audioDailyRewardWheel, SoundType.DailyRewardWheel));
            model.clips.Add(new SoundAudioClip(resource.audioDailyTaskNotification, SoundType.DailyTaskNotification));
            
            model.clips.Add(new SoundAudioClip(resource.audioCoin, SoundType.Coin));
            model.clips.Add(new SoundAudioClip(resource.audioPoint, SoundType.Point));
            model.clips.Add(new SoundAudioClip(resource.audioBrake, SoundType.Brake));
            model.clips.Add(new SoundAudioClip(resource.audioJump, SoundType.Jump));
            model.clips.Add(new SoundAudioClip(resource.audioTurn, SoundType.Turn));
            model.clips.Add(new SoundAudioClip(resource.audioSaw, SoundType.Saw));
            model.clips.Add(new SoundAudioClip(resource.audioEngine, SoundType.Engine));
            model.clips.Add(new SoundAudioClip(resource.audioSpeedUp, SoundType.SpeedUp));
            model.clips.Add(new SoundAudioClip(resource.audioSlowDown, SoundType.SlowDown));
            
            model.clips.Add(new SoundAudioClip(resource.audioSmash1, SoundType.Smash1));
            model.clips.Add(new SoundAudioClip(resource.audioSmash2, SoundType.Smash2));
            
            model.clips.Add(new SoundAudioClip(resource.audioStartLinePrepare, SoundType.StartLinePrepare));
            model.clips.Add(new SoundAudioClip(resource.audioStartLineRun, SoundType.StartLineRun));
            model.clips.Add(new SoundAudioClip(resource.audioLose, SoundType.Lose));
            model.clips.Add(new SoundAudioClip(resource.audioWin, SoundType.Win));
        }

        public void Play(SoundType type)
        {
            if (model.IsPlay())
            {
                AudioClip clip = model.GetClip(type);
                if (clip != null)
                    StartCoroutine(CreateNewSoundPlayer(clip));
            }
        }

        public void Play(AudioClip clip)
        {
            if (model.IsPlay() && clip != null)
                StartCoroutine(CreateNewSoundPlayer(clip));
        }
        private IEnumerator CreateNewSoundPlayer(AudioClip clip)
        {
            GameObject newPlayer = Instantiate(model.soundAudioPlayer);
            newPlayer.GetComponent<SoundAudioPlayer>().LoadAndPlay(clip);
            yield return null;
        }
    }
}
