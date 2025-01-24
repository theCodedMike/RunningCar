using Data;
using UnityEngine;

namespace Game.Game
{
    public enum GameState
    {
        PreGamePause, Pause, Game, Complete, Tutorial, RewardAfterLose
    }
    
    public class GameModel : MonoBehaviour
    {
        [HideInInspector]
        public ObjectsHolder objectsHolder;
        [HideInInspector]
        public SettingsHolder settingsHolder;
        
        [Header("游戏状态")]
        public GameState state = GameState.PreGamePause;
        [HideInInspector]
        public bool isFirstLose = true;
        
        public bool isTutorial;
        public int completedGrounds;


        private void Awake()
        {
            objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
            settingsHolder = SettingsHolder.Instance;
        }
    }
}
