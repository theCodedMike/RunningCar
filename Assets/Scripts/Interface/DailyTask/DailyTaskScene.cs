using System;
using System.DailyTask;
//using System.MobileFeatures.Ad;
using System.Utility;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.DailyTask
{
    public class DailyTaskScene : MonoBehaviour
    {
        [Header("组件")]
        private SettingsHolder _settingsHolder;
        public ObjectsHolder objectsHolder;
        public DailyTaskManager dailyTaskManager;

        [Header("当前任务")]
        public TextMeshProUGUI textDayIndicator;
        public TextMeshProUGUI textDescription;
        public GameObject canvasReward;
        public Text coinRewardValue;
        [Space()]
        public Image progressBarState;
        public TextMeshProUGUI textProgressBarState;
        [Space()]
        public TextMeshProUGUI buttonReceive;
        [Space()]
        public GameObject overlayCompleted;

        [Header("Strike")]
        public DayStrikeIndicator[] dayStrikeIndicators;
        public Slider progressStrike;

        private bool _isReadyToUpdateButton;
        private bool _isRewardReady;
        private bool _isCompleted;
        private bool _isDoubled;


        private void Awake()
        {
            if (_settingsHolder == null)
                _settingsHolder = SettingsHolder.Instance;
        }

        private void Update()
        {
            UpdateButtonReceive();
        }

        #region View

        public void PrepareViews()
        {
            System.DailyTask.DailyTask task = dailyTaskManager.GetCurrentTask();
            int completed = dailyTaskManager.progress;
            if (completed >= task.values)
                completed = task.values;
            int daysCompleted = CustomPlayerPrefs.GetInt("dailyTask_daysCompleted", 0);

            _isRewardReady = CustomPlayerPrefs.GetBool("dailyTask_rewardReady", false);
            _isCompleted = CustomPlayerPrefs.GetBool("dailyTask_isCompleted", false);

            // 更新当前任务
            textDayIndicator.text = $"Day {(_isCompleted ? daysCompleted : daysCompleted + 1)}";
            textDescription.text = task.GetTaskDescription();
            textProgressBarState.text = $"{completed}/{task.values}";
            progressBarState.fillAmount = 1f / task.values * completed;
            
            // 更新奖励
            for (int i = 0; i < dayStrikeIndicators.Length; i++)
                dayStrikeIndicators[i].Load(_settingsHolder.settingsGame.dailyTaskRewards[i], daysCompleted, i);
            progressStrike.value = 1f / 6 * (daysCompleted + 1);

            _isReadyToUpdateButton = true;
        }

        public void UpdateButtonReceive()
        {
            if (!_isReadyToUpdateButton)
                return;
            if (_isRewardReady)
            {
                buttonReceive.text = "接受";
                overlayCompleted.SetActive(false);
            }
            else
            {
                if (_isCompleted)
                {
                    buttonReceive.text = GetTimeToEndOfDay();
                    overlayCompleted.SetActive(true);
                }
                else
                {
                    buttonReceive.text = "完成任务";
                    overlayCompleted.SetActive(false);
                }
            }
        }

        private string GetTimeToEndOfDay()
        {
            DateTime date = DateTime.UtcNow;
            DateTime dateNext = date.Date.AddDays(1);
            TimeSpan remaining = dateNext - date;
            
            string hours = remaining.Hours < 10 ? $"0{remaining.Hours}" : remaining.Hours.ToString();
            string minutes = remaining.Minutes < 10 ? $"0{remaining.Minutes}" : remaining.Minutes.ToString();
            string seconds = remaining.Seconds < 10 ? $"0{remaining.Seconds}" : remaining.Seconds.ToString();

            return $"{hours}:{minutes}:{seconds}";
        }
        #endregion


        public void Receive()
        {
            if (!_isRewardReady)
                return;

            if (_settingsHolder.settingsAds.showDoubleCoinsAtDailyTask)
                ShowCanvasReward();
            else
                GetReward();
        }



        #region PopUp

        public void ShowCanvasReward()
        {
            canvasReward.gameObject.SetActive(true);
            coinRewardValue.text = dailyTaskManager.GetRewardValue().ToString();
        }

        public void HideCanvasReward() => canvasReward.gameObject.SetActive(false);

        public void GetReward()
        {
            _isRewardReady = false;
            dailyTaskManager.GetReward(_isDoubled);
            _isDoubled = false;
            
            HideCanvasReward();
            PrepareViews();
        }

        /*
        public void DoubleReward() => objectsHolder.adsInterface.ShowReward(AdRewardType.DailyTask);
        */
        
        public void ReceiveDoubleReward()
        {
            _isDoubled = true;
            GetReward();
        }

        #endregion
    }
}
