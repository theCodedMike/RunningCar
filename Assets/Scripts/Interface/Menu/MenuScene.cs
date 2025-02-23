using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.Utility;
using Data;
using Interface.Scenes;
using ScriptableObjects.Level;
using TMPro;
using UnityEngine;

namespace Interface.Menu
{
    public class MenuScene : MonoBehaviour
    {
        private SettingsHolder _settingsHolder;

        public LevelHolder levelHolder;
        public ScenesController scenesController;

        [Space()]
        public GameObject buttonDailyReward;
        public GameObject indicatorDailyReward;

        public GameObject buttonDailyTask;
        public GameObject indicatorDailyTask;
        public TextMeshProUGUI textIndicatorDailyTask;

        private void Awake()
        {
            if(_settingsHolder == null)
                _settingsHolder = SettingsHolder.Instance;
        }

        private void Start()
        {
            UpdateButtonDailyReward();
            UpdateButtonDailyTask();
        }

        public bool RunLevel(int id)
        {
            if (levelHolder.IsLevelUnlocked(id))
            {
                if (CustomPlayerPrefs.GetBool("tutorialWasOpened", false))
                {
                    levelHolder.RunLevel(id);
                    scenesController.ChangeScene("GameScene");
                }
                else
                {
                    CustomPlayerPrefs.SetBool("tutorialWasOpened", true);
                    scenesController.ChangeScene("TutorialScene");
                }

                return true;
            }

            return false;
        }

        private void UpdateButtonDailyReward()
        {
            if(_settingsHolder.settingsGame.dailyRewardIsOn)
                indicatorDailyReward.SetActive(DailyReward.DailyReward.IsAvailable());
            else
                buttonDailyReward.SetActive(false);
        }

        private void UpdateButtonDailyTask()
        {
            if (_settingsHolder.settingsGame.dailyTaskIsOn)
            {
                if (CustomPlayerPrefs.GetBool("dailyTask_rewardReady", false))
                {
                    indicatorDailyTask.SetActive(true);
                    textIndicatorDailyTask.text = "接受奖励吧";
                    Sound.Play(SoundType.DailyTaskNotification);
                }
            } else
                buttonDailyTask.SetActive(false);
        }

        public void ShowNewDailyTaskNotification()
        {
            if(!_settingsHolder.settingsGame.dailyTaskIsOn)
                return;
            
            indicatorDailyTask.SetActive(true);
            textIndicatorDailyTask.text = "新任务";
            Sound.Play(SoundType.DailyTaskNotification);
        }
    }
}
