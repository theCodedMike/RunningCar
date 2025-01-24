namespace System.Utility
{
    [Serializable]
    public class Counter
    {
        public int value;

        private int _valueDefault;
        private bool _isValueDefaultSet;

        public Counter(int value)
        {
            this.value = value;
        }


        #region Value

        public bool PerformAndCheck()
        {
            Decrease();
            bool isReady = IsReady();
            if(isReady)
                Restore();
            return isReady;
        }

        public void Decrease()
        {
            TryToSetDefault();
            value--;
        }

        public bool IsReady() => value < 0;

        public void Restore() => value = _valueDefault;

        #endregion





        #region State

        private void TryToSetDefault()
        {
            if (!_isValueDefaultSet)
            {
                _isValueDefaultSet = true;
                _valueDefault = value;
            }
        }
        #endregion
    }
}
