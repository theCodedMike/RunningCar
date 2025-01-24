using System.Collections;
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
    }
}
