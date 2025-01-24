using System.Audio.Sound;
using System.Audio.Sound.Audio;
using System.Collections.Generic;
using System.Utility;
using Data;
using Interface.DailyTask;
using Interface.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.DailyTask
{
    public class DailyTaskManager : MonoBehaviour
    {
        [Header("组件")]
        public ObjectsHolder objectsHolder;
        [Header("Value")]
        public DailyTaskType taskType;
        public int goal;
        public int progress = 0;
        public int objType = 0;
        public bool isCompleted = false;
        
        private SettingsHolder _settingsHolder;


        private void Start()
        {
            if(_settingsHolder == null)
                _settingsHolder = SettingsHolder.Instance;
         
            GetTask();
            if (SceneManager.GetActiveScene().name == "DailyTaskScene")
            {
                GameObject scene = GameObject.Find("DailyTaskScene");
                if(scene != null)
                    scene.GetComponent<DailyTaskScene>().PrepareViews();
            }
        }

        #region State

        public void GetTask()
        {
            string date = CustomPlayerPrefs.GetString("dailyTask_dayGenerated", string.Empty);
            string currentDate = GetDate();
            
            if(!IsNextDay())
                DropStrikeRow();
            
            if(date == currentDate)
                LoadTask();
            else
                GenerateTask();
        }

        public DailyTask GetCurrentTask() => new DailyTask(taskType, goal);
        #endregion



        #region Work with Task

        private void LoadTask()
        {
            taskType = (DailyTaskType)Enum.Parse(typeof(DailyTaskType), CustomPlayerPrefs.GetString("dailyTask_task"));
            goal = CustomPlayerPrefs.GetInt("dailyTask_goal");
            progress = CustomPlayerPrefs.GetInt("dailyTask_progress");
            objType = CustomPlayerPrefs.GetInt("dailyTask_objType");
            isCompleted = CustomPlayerPrefs.GetBool("dailyTask_isCompleted");
        }

        private void GenerateTask()
        {
            List<DailyTask> dailyTasks = _settingsHolder.settingsGame.dailyTasks;
            DailyTask randomTask = dailyTasks[RandomInt.GetRandom(dailyTasks.Count - 1)];

            taskType = randomTask.type;
            goal = randomTask.values;
            
            CustomPlayerPrefs.SetString("dailyTask_dayGenerated", GetDate());
            CustomPlayerPrefs.SetString("dailyTask_previousTaskDate", DateTime.UtcNow.ToString());
            CustomPlayerPrefs.SetBool("dailyTask_rewardReady", false);
            progress = 0;
            SaveTask();

            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                GameObject scene = GameObject.Find("MenuScene");
                if (scene != null)
                {
                    Sound.Play(SoundType.DailyTaskNotification);
                    scene.GetComponent<MenuScene>().ShowNewDailyTaskNotification();
                }
            }
        }

        private void SaveTask()
        {
            CustomPlayerPrefs.SetString("dailyTask_task", taskType.ToString());
            CustomPlayerPrefs.SetInt("dailyTask_goal", goal);
            CustomPlayerPrefs.SetInt("dailyTask_progress", progress);
            CustomPlayerPrefs.SetInt("dailyTask_objType", objType);
            CustomPlayerPrefs.SetBool("dailyTask_isCompleted", isCompleted);
        }
        #endregion



        #region Process task

        public void IncreaseProgress(DailyTaskType taskToIncrease, int value)
        {
            if (taskType != taskToIncrease)
                return;

            progress += value;
            
            CheckIsComplete();
            SaveTask();
        }

        private void CheckIsComplete()
        {
            if(!isCompleted && progress >= goal)
                Complete();
        }
        private void Complete()
        {
            isCompleted = true;
            int strike = CustomPlayerPrefs.GetInt("dailyTask_daysCompleted", 0);
            if (strike < _settingsHolder.settingsGame.dailyTaskRewards.Count - 1)
                strike++;
            CustomPlayerPrefs.SetInt("dailyTask_daysCompleted", strike);
            
            CustomPlayerPrefs.SetBool("dailyTask_rewardReady", true);

            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                GameObject objsHolder = GameObject.Find("ObjectsHolder");
                if (objsHolder != null)
                {
                    Sound.Play(SoundType.DailyTaskNotification);
                    ObjectsHolder holder = objsHolder.GetComponent<ObjectsHolder>();
                    holder.canvasPopUpDailyTask.gameObject.SetActive(true);
                    holder.canvasPopUpDailyTask.PlayAnimation();
                }
            }
        }

        public void GetReward(bool doubled)
        {
            if (CustomPlayerPrefs.GetBool("dailyTask_rewardReady", false))
            {
                CustomPlayerPrefs.SetBool("dailyTask_rewardReady", false);
                int value = GetRewardValue();

                int coins = CustomPlayerPrefs.GetInt("coin");
                CustomPlayerPrefs.SetInt("coin", coins + (doubled ? value * 2 : value));
                
                objectsHolder.indicatorsController.UpdateIndicatorCoins();
                Sound.Play(SoundType.Coin);
            }
        }

        public int GetRewardValue()
        {
            int strike = CustomPlayerPrefs.GetInt("dailyTask_daysCompleted", 0);
            List<int> data = _settingsHolder.settingsGame.dailyTaskRewards;
            int value = data[strike - 1];
            return value;
        }
        #endregion



        #region Date
        private string GetDate() => DateTime.UtcNow.ToString("yyyy-MM-dd");

        public bool IsNextDay()
        {
            string prevTaskDateStr = CustomPlayerPrefs.GetString("dailyTask_previousTaskDate");
            if (string.IsNullOrEmpty(prevTaskDateStr))
                return false;

            DateTime prevTaskDate = DateTime.Parse(prevTaskDateStr);
            int hourStrike = HourStrikesBetween(prevTaskDate, DateTime.UtcNow);

            return hourStrike <= 25;
        }

        public int HourStrikesBetween(DateTime from, DateTime to)
        {
            if(from > to)
                throw new ArgumentException("from must not be after to");

            DateTime fromTrimmed = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTime toTrimmed = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);
            
            return (int)(toTrimmed - fromTrimmed).TotalHours;
        }

        public void DropStrikeRow() => CustomPlayerPrefs.SetInt("dailyTask_daysCompleted", 0);

        #endregion
    }
}
