using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsAudio", menuName = "RunningCar/Settings/Audio", order = 22)]
    public class SettingsAudio : ScriptableObject
    {
        [Header("Audio")]
        public AudioClip audioButtonClick;
        public AudioClip audioSwitch;
        public AudioClip audioError;
        public AudioClip audioSuccessful;
        public AudioClip audioEquip;
        
        [Space()]
        public AudioClip audioDailyRewardWheel;
        public AudioClip audioDailyTaskNotification;
        
        [Space()]
        public AudioClip audioCoin;
        public AudioClip audioPoint;
        public AudioClip audioBrake;
        public AudioClip audioJump;
        public AudioClip audioTurn;
        public AudioClip audioSaw;
        public AudioClip audioEngine;
        public AudioClip audioSpeedUp;
        public AudioClip audioSlowDown;
        
        [Space()]
        public AudioClip audioSmash1;
        public AudioClip audioSmash2;
        
        [Space()]
        public AudioClip audioStartLinePrepare;
        public AudioClip audioStartLineRun;
        public AudioClip audioLose;
        public AudioClip audioWin;
        
        [Header("Music")]
        public AudioClip musicGame;
        public AudioClip musicMenu;
    }
}
