namespace System.Utility
{
    static class RandomBool
    {
        public static bool GetRandom() => UnityEngine.Random.Range(0, 2) == 0;
    }

    static class RandomInt
    {
        public static int GetRandom(int from, int to) => UnityEngine.Random.Range(from, to + 1);
        
        public static int GetRandom(int value) => UnityEngine.Random.Range(0, value + 1);
    }

    static class RandomFloat
    {
        public static float GetRandom(float from, float to) => UnityEngine.Random.Range(from, to);
        
        public static float GetRandom(float value) => UnityEngine.Random.Range(0, value);
    }
}
