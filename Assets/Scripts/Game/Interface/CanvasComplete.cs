using System.Audio.Sound;
using Data;
using ScriptableObjects.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interface
{
    public enum State
    {
        Win, Lose
    }
    public class CanvasComplete : MonoBehaviour
    {
        [Header("Objects")]
        public ObjectsHolder objectsHolder;

        [Header("组件")]
        public TextMeshProUGUI textCurrentLevel;
        public TextMeshProUGUI textNotification;
        public IndicatorStars indicatorStars;
        [Space()]
        public GameObject buttonNextLevel;
        [Space()]
        public Image background;
        
        [Header("Values")]
        public Sprite starActive;
        public Sprite starInactive;
        [Space()]
        public Sprite backgroundWin;
        public Sprite backgroundLose;

        [Header("音频")]
        public AudioClip audioWin;
        public AudioClip audioLose;

        public void Show(State state, bool isUnlockedNextLevel, int rating)
        {
            Level nextLevel = objectsHolder.levelHolder.ReturnNextLevel();
            int currentLevelId = objectsHolder.levelHolder.ReturnRunnedLevelId();
            bool nextLevelUnlocked = objectsHolder.levelHolder.IsNextLevelUnlocked();

            background.sprite = state == State.Win ? backgroundWin : backgroundLose;
            textCurrentLevel.text = $"关卡{currentLevelId + 1}";
            if (isUnlockedNextLevel)
                textNotification.text = "下一关卡已解锁哦！";
            else
                textNotification.text = state == State.Win ? "进行下一关吧！" : "再试一次吧！";

            if (nextLevel == null)
                textNotification.text = "恭喜你，完成了所有关卡！";
            indicatorStars.SetStar(rating);
            buttonNextLevel.SetActive(nextLevelUnlocked);
            
            Sound.Play(state == State.Win ? audioWin : audioLose);
        }
    }
}
