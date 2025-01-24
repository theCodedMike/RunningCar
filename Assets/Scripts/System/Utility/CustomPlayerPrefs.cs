using UnityEngine;

namespace System.Utility
{
    public class CustomPlayerPrefs : PlayerPrefs
    {
        private const string IDArrayLength = "_custom_arrays_length_";
        private const string IDArrayObjectID = "_custom_arrays_object_id_";

        #region Boolean
        public static void SetBool(string key, bool value) => SetInt(key, value ? 1 : 0);
        public static bool GetBool(string key) => GetInt(key) == 1;
        public static bool GetBool(string key, bool defaultValue) => GetInt(key, defaultValue ? 1 : 0) == 1;

        public static void SetBoolArray(string key, bool[] values)
        {
            SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                SetInt(key + IDArrayObjectID + index, values[index] ? 1 : 0);
        }
        public static bool[] GetBoolArray(string key)
        {
            int length = GetInt(key + IDArrayLength, 0);
            bool[] values = new bool[length];
            for (int index = 0; index < length; index++)
                values[index] = GetInt(key + IDArrayObjectID + index) == 1;
            return values;
        }
        #endregion

        #region Float
        public static void SetFloatArray(string key, float[] values)
        {
            SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                SetFloat(key + IDArrayObjectID + index, values[index]);
        }
        public static float[] GetFloatArray(string key)
        {
            int length = GetInt(key + IDArrayLength, 0);
            float[] values = new float[length];
            for (int index = 0; index < length; index++)
                values[index] = GetFloat(key + IDArrayObjectID + index);
            return values;
        }
        #endregion
        
        #region Int
        public static void SetIntArray(string key, int[] values)
        {
            SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                SetInt(key + IDArrayObjectID + index, values[index]);
        }
        public static int[] GetIntArray(string key)
        {
            int length = GetInt(key + IDArrayLength, 0);
            int[] values = new int[length];
            for (int index = 0; index < length; index++)
                values[index] = GetInt(key + IDArrayObjectID + index);
            return values;
        }
        #endregion
        
        #region String
        public static void SetStringArray(string key, string[] values)
        {
            SetInt(key + IDArrayLength, values.Length);
            for(int index = 0; index < values.Length; index++)
                SetString(key + IDArrayObjectID + index, values[index]);
        }
        public static string[] GetStringArray(string key)
        {
            int length = GetInt(key + IDArrayLength, 0);
            string[] values = new string[length];
            for (int index = 0; index < length; index++)
                values[index] = GetString(key + IDArrayObjectID + index);
            return values;
        }
        #endregion
    }
}
