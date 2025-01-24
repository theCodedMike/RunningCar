namespace System.Utility
{
    [Serializable]
    public class GeneratorPoint
    {
        private float _current;
        private const float Start = 0f;
        private const float Distance = 20f;
        private const float Shift = 1f;

        public GeneratorPoint()
        {
            _current = Start;
        }

        public float GetValue() => _current;

        public bool IsReachDistance(float value, bool isIncrease)
        {
            if (isIncrease)
                return value + Distance > _current;
            else
                return value - Distance < _current;
        }

        public void Change() => _current += Shift;
    }
}
