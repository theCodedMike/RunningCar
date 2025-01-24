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
            DailyTaskType.MakeTurns => $"Make {values} turns during a game.",
            DailyTaskType.MakeJumps => $"Make {values} jumps during a game.",
            DailyTaskType.FinishLevels => $"Finished {values} times.",
            DailyTaskType.CollectCoins => $"Collects {values} diamonds on levels.",
            DailyTaskType.CollectRating => $"Catch {values} stars on levels.",
            _ => ""
        };
    }
}
