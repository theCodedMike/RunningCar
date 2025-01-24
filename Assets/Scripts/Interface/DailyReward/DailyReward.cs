using System;
using System.Utility;

namespace Interface.DailyReward
{
    public static class DailyReward
    {
        public static bool IsAvailable()
        {
            string completed = CustomPlayerPrefs.GetString("dailyReward_Completed", string.Empty);
            return GetDate() != completed;
        }

        public static string GetDate() => DateTime.UtcNow.ToString("yyyy-MM-dd");
    }
}
