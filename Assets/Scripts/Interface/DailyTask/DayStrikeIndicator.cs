using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interface.DailyTask
{
    public class DayStrikeIndicator : MonoBehaviour
    {
        [Header("组件")]
        public TextMeshProUGUI textDay;
        public TextMeshProUGUI textValue;
        [Space()]
        public Image icon;

        [Header("值")]
        public Sprite spriteCompleted;
        public Sprite spriteCurrent;
        public Sprite spriteWait;

        public void Load(int reward, int dayCompleted, int indicatorDay)
        {
            textValue.text = reward.ToString();
            icon.sprite = indicatorDay == dayCompleted ? spriteCurrent :
                dayCompleted < indicatorDay ? spriteWait : spriteCompleted;
        }
    }
}
