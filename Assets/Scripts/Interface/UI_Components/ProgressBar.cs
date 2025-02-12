using UnityEngine;
using UnityEngine.UI;

namespace Interface.UI_Components
{
    public class ProgressBar : MonoBehaviour
    {
        public Image top;
        
        [Space()]
        public bool isAnimated = true;
        public float animationSpeed = 0.0005f;
        [Space()]
        public float targetValue = 0f;


        private void Update()
        {
            if (isAnimated)
                UpdateValues();
        }

        public void SetValue(float value)
        {
            targetValue = value;
            UpdateValues();
        }

        public void SetValueInstant(float value)
        {
            targetValue = value;
            top.fillAmount = targetValue;
        }

        public void UpdateValues()
        {
            if (isAnimated)
            {
                if (Mathf.Approximately(top.fillAmount, targetValue))
                    return;

                float changeBy = top.fillAmount < targetValue ? animationSpeed : -animationSpeed;
                float diff = top.fillAmount < targetValue ? targetValue - top.fillAmount : top.fillAmount - targetValue;
                print($"ProgressBar: changeBy{changeBy}, diff{diff}, fillAmount{top.fillAmount}, targetValue{targetValue}");
                if (diff < changeBy)
                    changeBy = changeBy < 0 ? -diff : diff;

                top.fillAmount += changeBy;
            }
            else
                top.fillAmount = targetValue;
        }
    }
}
