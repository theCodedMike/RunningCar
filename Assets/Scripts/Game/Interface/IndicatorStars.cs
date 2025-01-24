using UnityEngine;
using UnityEngine.UI;

namespace Game.Interface
{
    public class IndicatorStars : MonoBehaviour
    {
        public Image start1;
        public Image start2;
        public Image start3;

        public Sprite startActive;
        public Sprite startInactive;

        public void SetStar(int value)
        {
            start1.sprite = value >= 1 ? startActive : startInactive;
            start2.sprite = value >= 2 ? startActive : startInactive;
            start3.sprite = value >= 3 ? startActive : startInactive;
        }
    }
}
