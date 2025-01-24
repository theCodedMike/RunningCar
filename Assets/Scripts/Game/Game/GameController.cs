using System.DailyTask;
using Game.Interface;
using Game.Objects.Player.Controllers;
using UnityEngine;

namespace Game.Game
{
    [RequireComponent(typeof(GameModel))]
    [RequireComponent(typeof(GameView))]
    public class GameController : MonoBehaviour
    {
        private GameModel _model;
        private GameView _view;

        #region Standard system methods
        private void Awake()
        {
            _model = GetComponent<GameModel>();
            _view = GetComponent<GameView>();
        }

        private void Start()
        {
            _view.ChangeInterfaceState();
            if (_model.isTutorial)
                return;
            
            _model.objectsHolder.levelLoader.Load();
            _model.objectsHolder.gameObjectLoader.Load();
        }
        #endregion



        
        
        #region State

        public void GameStart()
        {
            _model.state = GameState.Game;
            _view.ChangeInterfaceState();
        }

        public void GameOver()
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            _model.state = GameState.Complete;
            _view.ChangeInterfaceState();
#else
            if (_model.isFirstLose && _model.settingsHolder.settingsAds.showRewardVideoWhenLose &&
                _model.completedGrounds > 1)
            {
                _model.isFirstLose = false;
                _model.state = GameState.RewardAfterLose;
                _view.ChangeInterfaceState();
            }
            else
            {
                _model.state = GameState.Complete;
                _view.ChangeInterfaceState();
            }
#endif
        }

        public void ContinueGame()
        {
            _model.state = GameState.PreGamePause;
            _view.ChangeInterfaceState();
            
            _model.objectsHolder.gameObjectLoader.player.GetComponent<PlayerController>().RestorePlayer();
            _model.objectsHolder.canvasPreGamePause.Restart();
        }

        public void Pause()
        {
            _model.state = _model.state == GameState.Game ? GameState.Pause : GameState.Game;
            Time.timeScale = _model.state == GameState.Pause ? 0 : 1;
            
            _view.ChangeInterfaceState();
        }
        
        public void RemovePauseForBackToMenu() => Time.timeScale = 1;

        public void Lose()
        {
            if (_model.isTutorial)
            {
                _model.objectsHolder.scenesController.ChangeSceneWithDelay("TutorialScene");
                return;
            }
            
            GameOver();
            _model.objectsHolder.canvasComplete.Show(State.Lose, false, 0);
        }

        public void Win()
        {
            if (_model.isTutorial)
            {
                _model.objectsHolder.scenesController.ChangeSceneWithDelay("MenuScene");
                return;
            }

            _model.completedGrounds = _model.objectsHolder.levelLoader.groundCount;
            _model.objectsHolder.dailyTaskManager.IncreaseProgress(DailyTaskType.FinishLevels, 1);
            UpdateGroundComplete();

            int rating = _model.objectsHolder.gameData.rating;
            _model.objectsHolder.levelHolder.ReturnRunnedLevel().SetResult(rating);

            bool isNextLevelUnlocked = _model.objectsHolder.levelHolder.TryToUnlockNextLevel();
            _model.objectsHolder.canvasComplete.Show(State.Win, isNextLevelUnlocked, rating);

            _model.isFirstLose = false;
            GameOver();
        }

        public void RunNextLevel()
        {
            _model.objectsHolder.levelHolder.RunNextLevel();
            _model.objectsHolder.scenesController.ChangeSceneWithDelay("GameScene");
        }
        
        public GameState GetState() => _model.state;

        public void LoadTutorial(string title, string message, Sprite icon)
        {
            _model.state = GameState.Tutorial;
            _view.ChangeInterfaceState();
            _model.objectsHolder.canvasTutorial.Load(title, message, icon);
        }

        public void CloseTutorial()
        {
            _model.state = GameState.Game;
            _view.ChangeInterfaceState();
        }
        #endregion




        #region Values
        public void IncreaseGroundCount()
        {
            _model.completedGrounds++;
            UpdateGroundComplete();
        }

        public void UpdateGroundComplete()
        {
            int maxGrounds = _model.objectsHolder.levelLoader.groundCount;
            int currentGrounds = _model.completedGrounds;
            if (currentGrounds > maxGrounds)
                currentGrounds = maxGrounds;
            
            _model.objectsHolder.progressBarLevel.SetValue(1f / maxGrounds * currentGrounds);
        }
        #endregion
    }
}
