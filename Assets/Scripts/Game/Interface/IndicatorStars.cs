using UnityEngine;
using UnityEngine.UI;

namespace Game.Interface
{
    public class IndicatorStars : MonoBehaviour
    {
        public Image star1;
        public Image star2;
        public Image star3;

        public Sprite starActive;
        public Sprite starInactive;

        public void SetStar(int value)
        {
            star1.sprite = value >= 1 ? starActive : starInactive;
            star2.sprite = value >= 2 ? starActive : starInactive;
            star3.sprite = value >= 3 ? starActive : starInactive;
        }
    }
}
