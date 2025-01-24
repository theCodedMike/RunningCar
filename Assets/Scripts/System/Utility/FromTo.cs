namespace System.Utility
{
    [Serializable]
    public struct FromToInt
    {
        public int from;
        public int to;

        public FromToInt(int from, int to)
        {
            this.from = from;
            this.to = to;
        }

        #region Get
        public int GetRandom() => UnityEngine.Random.Range(from, to);
        #endregion
    }
    
    [Serializable]
    public struct FromToFloat
    {
        public float from;
        public float to;

        public FromToFloat(float from, float to)
        {
            this.from = from;
            this.to = to;
        }
        #region Get
        public float GetRandom() => UnityEngine.Random.Range(from, to);

        public float GetValueByPercentage(float percentage)
        {
            bool normal = from < to;
            float diff = normal ? to - from : from - to;
            float value = normal ? from + (diff / 1f * percentage) : from - (diff / 1f * percentage);
            return value;
        }
        #endregion
    }
}
