using Data;
using UnityEngine;

namespace Game.Game
{
    public class GameView : MonoBehaviour
    {
        private GameModel _model;
        
        private void Awake()
        {
            _model = GetComponent<GameModel>();
        }

        #region State

        public void ChangeInterfaceState()
        {
            GameState state = _model.state;
            ObjectsHolder holder = _model.objectsHolder;
            
            holder.canvasPause.SetActive(state == GameState.Pause);
            holder.canvasPreGamePause.gameObject.SetActive(state == GameState.PreGamePause);
            holder.canvasComplete.gameObject.SetActive(state == GameState.Complete);
            holder.canvasTutorial.gameObject.SetActive(state == GameState.Tutorial);
            holder.canvasRewardAfterLose.gameObject.SetActive(state == GameState.RewardAfterLose);
        }
        #endregion
    }
}
