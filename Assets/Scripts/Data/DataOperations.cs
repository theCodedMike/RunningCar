
using System.Utility;

namespace Data
{
    public static class DataOperations
    {
        public static void ChangeCoin(int value)
        {
            int coin = CustomPlayerPrefs.GetInt("coin") + value;
            CustomPlayerPrefs.SetInt("coin", coin);
        }
    }
}
