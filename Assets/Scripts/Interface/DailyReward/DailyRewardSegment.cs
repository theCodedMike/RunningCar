using UnityEngine;
using UnityEngine.UI;

namespace Interface.DailyReward
{
    public class DailyRewardSegment : MonoBehaviour
    {
        public GameObject coin;
        public GameObject nothing;
        [Space()]
        public Text textCoin;

        public void Load(DailyRewardData data)
        {
            switch (data.type)
            {
                case DailyRewardType.Coin:
                    coin.SetActive(true);
                    nothing.SetActive(false);
                    textCoin.text = data.value.ToString();
                    break;
                default:
                    coin.SetActive(false);
                    nothing.SetActive(true);
                    break;
            }
        }
    }
}
