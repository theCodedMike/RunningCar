using UnityEngine;

namespace System.Audio.Music
{
    public class Music : MonoBehaviour
    {
        public MusicType playHere = MusicType.Menu;
        
        private MusicPlayerController _musicPlayerController;

        private void Start()
        {
            _musicPlayerController = MusicPlayerController.Instance;
            LoadMusicForThisScene();
        }

        public void UpdateState()
        {
            MusicPlayerController.Instance.Play();
        }

        private void LoadMusicForThisScene()
        {
            MusicType currentType = _musicPlayerController.GetMusicType();
            if(playHere != currentType)
                _musicPlayerController.PlayMusic(playHere);
        }
    }
}
