namespace System.DailyTask
{
    public enum DailyTaskType
    {
        MakeTurns, MakeJumps, FinishLevels, CollectCoins, CollectRating
    }
    
    [Serializable]
    public class DailyTask
    {
        public DailyTaskType type;
        public int values;

        public DailyTask(DailyTaskType type, int values)
        {
            this.type = type;
            this.values = values;
        }
        
        public string GetTaskDescription() => type switch
        {
            DailyTaskType.MakeTurns => $"完成{values}次转弯。",
            DailyTaskType.MakeJumps => $"完成{values}次跳跃。",
            DailyTaskType.FinishLevels => $"完成{values}关。",
            DailyTaskType.CollectCoins => $"收集{values}个钻石。",
            DailyTaskType.CollectRating => $"收集{values}个星星。",
            _ => ""
        };
    }
}
