using System;

namespace Interface.DailyReward
{
    public enum DailyRewardType
    {
        Nothing, Coin,
    }
    
    [Serializable]
    public class DailyRewardData
    {
        public DailyRewardType type;
        public int value;
    }
}
