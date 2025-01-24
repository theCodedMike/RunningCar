using UnityEngine;

namespace System.Utility
{
    [Serializable]
    public class GameVariable
    {
        public float value = 2f;
        public float max = 4f;

        [Range(0, 100)]
        public float changeByPercent = 2f;


        private bool _isIncrease;
        private bool _isActive = true;

        #region Work with value

        public GameVariable()
        {
            _isIncrease = value < max;
        }

        public GameVariable(float value, float max, float changeByPercent)
        {
            this.value = value;
            this.max = max;
            this.changeByPercent = changeByPercent;

            _isIncrease = this.value < this.max;
        }

        public void ChangeValue()
        {
            if (_isActive)
            {
                float changeBy = value / 100 * changeByPercent;
                value += _isIncrease ? changeBy : -changeBy;
                if (_isIncrease ? value >= max : value <= max)
                    StopActivity();
            }
        }

        private void StopActivity()
        {
            _isActive = false;
            value = max;
        }
        #endregion
    }
}
