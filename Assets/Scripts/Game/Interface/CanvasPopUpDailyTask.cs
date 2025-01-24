using UnityEngine;

namespace Game.Interface
{
    public class CanvasPopUpDailyTask : MonoBehaviour
    {
        [Header("动画组件")]
        public Animation animationPanel;

        public void PlayAnimation() => animationPanel.Play();
    }
}
