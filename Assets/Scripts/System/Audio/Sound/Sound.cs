using System.Audio.Sound.Audio;
using UnityEngine;

namespace System.Audio.Sound
{
    public class Sound : MonoBehaviour
    {
        public static void Play(SoundType type)
            => SoundPlayerController.Instance.Play(type);
        
        public static void Play(AudioClip clip)
            => SoundPlayerController.Instance.Play(clip);

        public static void PlaySoundButtonClick()
            => SoundPlayerController.Instance.Play(SoundType.ButtonClick);
        
        public static void PlaySoundSwitch()
            => SoundPlayerController.Instance.Play(SoundType.Switch);

        public static void PlaySoundIncreaseCoin()
            => Play(SoundType.Coin);

        public static void PlaySoundIncreaseScore()
            => Play(SoundType.Point);
    }
}
