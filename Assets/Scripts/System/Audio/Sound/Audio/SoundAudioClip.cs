using UnityEngine;

namespace System.Audio.Sound.Audio
{
    public enum SoundType
    {
        ButtonClick, 
        Switch,
        Error,
        Successful,
        Equip,
        
        DailyRewardWheel,
        DailyTaskNotification,
        
        Coin,
        Point,
        Brake,
        Jump,
        Turn,
        Saw,
        Engine,
        SpeedUp,
        SlowDown,
        
        Smash1,
        Smash2,
        
        StartLinePrepare,
        StartLineRun,
        Lose,
        Win,
    }
    
    [Serializable]
    public class SoundAudioClip
    {
        public AudioClip audioClip;
        public SoundType audioType;

        public SoundAudioClip(AudioClip audioClip, SoundType audioType)
        {
            this.audioClip = audioClip;
            this.audioType = audioType;
        }
    }
}
