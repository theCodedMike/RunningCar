using System;
using System.Collections.Generic;
using System.Utility;
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Level
{
    [Serializable]
    public class Level
    {
        private static SettingsEditor _settingsEditor;
        private const string AssetsAddressSettingsEditor = "Assets/Scripts/ScriptableObjects/Settings/SettingsEditor.asset";

        public string name;
        public string id;
        public float sawSpeed = 5f;
        public List<LevelObject> objects;


        #region State
        public void ClearObjects() 
            => objects = new();
        public int GetResult() 
            => CustomPlayerPrefs.GetInt("levelResult_" + id, 0);

        public void SetResult(int value)
        {
            if(GetResult() < value)
                CustomPlayerPrefs.SetInt("levelResult_" + id, value);
        }
        public string GetStarStatus()
        {
            int count = 0;
            foreach (var level in objects)
            {
                if (level == null && level.prefab == null)
                    return null;
                if (level.prefab.transform.CompareTag("RatingStar"))
                    count++;
            }

            if (count < 3)
                return "is less < that 3 stars.";
            if(count > 3)
                return "is more > than 3 stars.";

            return null;
        }
        #endregion



        #region Work with objects
#if UNITY_EDITOR
        public void AddGameObjectToArray(GameObject prefab)
        {
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(AssetsAddressSettingsEditor);
            if (objects == null)
                objects = new();

            LevelObject levelObject = new LevelObject();
            levelObject.prefab = prefab;
            if (objects != null && objects.Count > 0)
            {
                LevelObject oldObject = objects[^1];
                if (_settingsEditor.levelEditorSetNewObjectToPreviousPosition)
                    levelObject.position = new Vector3(oldObject.position.x, levelObject.position.y, oldObject.position.z);
                if(_settingsEditor.levelEditorSetNewObjectToPreviousRotation)
                    levelObject.eulerAngles = new Vector3(oldObject.eulerAngles.x, oldObject.eulerAngles.y, oldObject.eulerAngles.z);
            }
            
            objects.Add(levelObject);
        }
#endif
        #endregion
    }
}
