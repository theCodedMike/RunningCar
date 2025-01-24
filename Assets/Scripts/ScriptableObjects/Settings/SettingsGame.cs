using System.Collections.Generic;
using System.DailyTask;
using System.Utility;
using Interface.DailyReward;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsGame", menuName = "RunningCar/Settings/Game", order = 25)]
    public class SettingsGame : ScriptableObject
    {
        [Header("Player")]
        public FromToFloat speed;
        public FromToFloat steering;
        public FromToFloat velocity;

        [Space]
        public float jumpDistance = 4.75f;
        public float jumpHeight = 1.25f;

        [Range(0, 1)]
        public float decreaseSpeedAtTurnTo = 0.35f;

        public float speedUp = 1.8f;
        public float speedDown = 0.5f;

        [Header("Daily Reward")]
        public bool dailyRewardIsOn = true;
        public DailyRewardData[] dailyRewardDatas;

        [Header("Daily Task")]
        public bool dailyTaskIsOn = true;
        public List<DailyTask> dailyTasks;
        public List<int> dailyTaskRewards;
    }
}
