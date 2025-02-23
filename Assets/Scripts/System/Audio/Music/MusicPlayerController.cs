using System.Collections;
using System.Utility;
using Interface.Settings;
using UnityEngine;

namespace System.Audio.Music
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MusicPlayerModel))]
    public class MusicPlayerController : MonoBehaviour
    {
        public static MusicPlayerController Instance { get; private set; }

        private MusicPlayerModel _model;



        #region Standard system methods
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;
                _model = GetComponent<MusicPlayerModel>();
            } else
                Destroy(gameObject);
        }

        private void Start()
        { 
            Play();   
        }
        #endregion




        public void Play()
        {
            if(_model.IsPlay())
                _model.audioSource.Play();
            else
                _model.audioSource.Pause();
        }

        public void PlayMusic(MusicType type)
        {
            StartCoroutine(LoadMusic(type));
        }

        private IEnumerator LoadMusic(MusicType type)
        {
            _model.type = type;
            ResetVolume();
            
            if (_model.IsPlay())
            {
                _model.audioSource.Stop();
                switch (_model.type)
                {
                    case MusicType.Game: _model.audioSource.clip = _model.musicGame; break;
                    case MusicType.Menu: _model.audioSource.clip = _model.musicMenu; break;
                }
                _model.audioSource.Play();
            } else
                _model.audioSource.Pause();

            yield return null;
        }

        public MusicType GetMusicType() => _model.type;

        public void ResetVolume()
        {
            float currVolume = _model.type == MusicType.Game 
                ? CustomPlayerPrefs.GetFloat(VolumeAdjust.GameVolume, 1f) 
                : CustomPlayerPrefs.GetFloat(VolumeAdjust.MenuVolume, 1f);
            
            _model.audioSource.volume = currVolume;
        }
    }
}
