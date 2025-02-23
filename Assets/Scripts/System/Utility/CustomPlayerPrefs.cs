using UnityEngine;

namespace System.Utility
{
    public static class CustomPlayerPrefs
    {
        private const string IDArrayLength = "_custom_arrays_length_";
        private const string IDArrayObjectID = "_custom_arrays_object_id_";

        #region Boolean
        public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
        public static bool GetBool(string key) => PlayerPrefs.GetInt(key) == 1;
        public static bool GetBool(string key, bool defaultValue) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

        public static void SetBoolArray(string key, bool[] values)
        {
            PlayerPrefs.SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                PlayerPrefs.SetInt(key + IDArrayObjectID + index, values[index] ? 1 : 0);
        }
        public static bool[] GetBoolArray(string key)
        {
            int length = PlayerPrefs.GetInt(key + IDArrayLength, 0);
            bool[] values = new bool[length];
            for (int index = 0; index < length; index++)
                values[index] = PlayerPrefs.GetInt(key + IDArrayObjectID + index) == 1;
            return values;
        }
        #endregion

        #region Float

        public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
        public static float GetFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);
        public static void SetFloatArray(string key, float[] values)
        {
            PlayerPrefs.SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                PlayerPrefs.SetFloat(key + IDArrayObjectID + index, values[index]);
        }
        public static float[] GetFloatArray(string key)
        {
            int length = PlayerPrefs.GetInt(key + IDArrayLength, 0);
            float[] values = new float[length];
            for (int index = 0; index < length; index++)
                values[index] = PlayerPrefs.GetFloat(key + IDArrayObjectID + index);
            return values;
        }
        #endregion
        
        #region Int
        public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public static int GetInt(string key) => PlayerPrefs.GetInt(key);
        public static int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
        public static void SetIntArray(string key, int[] values)
        {
            PlayerPrefs.SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                PlayerPrefs.SetInt(key + IDArrayObjectID + index, values[index]);
        }
        public static int[] GetIntArray(string key)
        {
            int length = PlayerPrefs.GetInt(key + IDArrayLength, 0);
            int[] values = new int[length];
            for (int index = 0; index < length; index++)
                values[index] = PlayerPrefs.GetInt(key + IDArrayObjectID + index);
            return values;
        }
        #endregion
        
        #region String
        public static void SetString(string key, string value) => PlayerPrefs.SetString(key, value);
        public static string GetString(string key) => PlayerPrefs.GetString(key);
        public static string GetString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
        public static void SetStringArray(string key, string[] values)
        {
            PlayerPrefs.SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                PlayerPrefs.SetString(key + IDArrayObjectID + index, values[index]);
        }
        public static string[] GetStringArray(string key)
        {
            int length = PlayerPrefs.GetInt(key + IDArrayLength, 0);
            string[] values = new string[length];
            for (int index = 0; index < length; index++)
                values[index] = PlayerPrefs.GetString(key + IDArrayObjectID + index);
            return values;
        }
        #endregion

        public static void Save() => PlayerPrefs.Save();
    }
}
