using System.Audio.Sound;
using System.DailyTask;
//using System.MobileFeatures.Ad;
using Game;
using Game.Game;
using Game.Interface;
using Interface.Indicator;
using Interface.Scenes;
using Interface.Store;
using Interface.UI_Components;
using ScriptableObjects.Level;
using ScriptableObjects.Skins;
using UnityEngine;

namespace Data
{
    public class ObjectsHolder : MonoBehaviour
    {
        [Header("游戏")]
        public GameModel gameModel;
        public GameController gameController;
        public GameObjectLoader gameObjectLoader;
        public LevelLoader levelLoader;
        public DailyTaskManager dailyTaskManager;

        [Header("UI")]
        public GameObject canvasPause;
        public CanvasPreGamePause canvasPreGamePause;
        public CanvasComplete canvasComplete;
        public CanvasTutorial canvasTutorial;
        public CanvasRewardAfterLose canvasRewardAfterLose;
        public CanvasPopUpDailyTask canvasPopUpDailyTask;
        
        [Space()]
        public IndicatorsController indicatorsController;
        public IndicatorStars indicatorStars;

        [Space()]
        public ProgressBar progressBarLevel;

        [Space()]
        public SkinPreview skinPreview;

        [Header("数据")]
        public SkinHolder skinHolder;
        public LevelHolder levelHolder;
        [Space()]
        public GameData gameData;
        
        [Header("Service")]
        public ScenesController scenesController;
        [Space()]
        public Sound sound;
        /*
        [Space()]
        public AdsInterface adsInterface;*/
    }
}
