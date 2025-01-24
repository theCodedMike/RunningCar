using System.Collections.Generic;
using System.Utility;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects.Level
{
    [CreateAssetMenu(fileName = "LevelHolder", menuName = "RunningCar/Level/LevelHolder", order = 25)]
    public class LevelHolder : ScriptableObject
    {
        public List<Level> levels;
        public int levelToLoad = 0;

        public Level GetLevelById(int id) => levels[id];

#if UNITY_EDITOR
        public void AddGamePrefab(string address, int levelId)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(address);
            AddGameObjectToLevel(prefab, levelId);
            SceneView.RepaintAll();
        }

        private void AddGameObjectToLevel(GameObject prefab, int levelId) =>
            levels[levelId].AddGameObjectToArray(prefab);
#endif


        #region States
        public List<string> GetLevelsStarStatus()
        {
            List<string> statusList = new();
            for (int i = 0; i < levels.Count; i++)
            {
                string status = levels[i].GetStarStatus();
                if (!string.IsNullOrEmpty(status))
                {
                    statusList.Add($"Level {i + 1}: {status}");
                }
            }
            return statusList;
        }

        public string GetStarStatusForLevelId(int id)
        {
            string status = levels[id].GetStarStatus();
            if (!string.IsNullOrEmpty(status))
                status = $"Level {status}";
            
            return status;
        }
        #endregion



        #region Access
        public bool IsLevelUnlocked(int id) => id <= ReturnLevelUnlockedId();

        public bool IsNextLevelUnlocked()
        {
            int nextLevelId = ReturnRunnedLevelId() + 1;
            return nextLevelId < levels.Count && nextLevelId <= ReturnLevelUnlockedId();
        }

        public bool TryToUnlockNextLevel()
        {
            int levelUnlockedId = ReturnLevelUnlockedId();
            if (ReturnRunnedLevelId() == levelUnlockedId)
            {
                CustomPlayerPrefs.SetInt("levelsUnlockedID", levelUnlockedId + 1);
                return true;
            }
            
            return false;
        }
        #endregion


        public void RunLevel(int id) => CustomPlayerPrefs.SetInt("levelLoaded", id);

        public Level ReturnRunnedLevel() => GetLevelById(ReturnRunnedLevelId());
        
        public Level ReturnNextLevel()
        {
            int nextLevelId = ReturnRunnedLevelId() + 1;
            return nextLevelId < levels.Count ? GetLevelById(nextLevelId) : null;
        }
        
        public int ReturnRunnedLevelId() => CustomPlayerPrefs.GetInt("levelLoaded", 0);
        
        public int ReturnLevelUnlockedId() => CustomPlayerPrefs.GetInt("levelsUnlockedID", 0);

        public void RunNextLevel()
        {
            int currentLevel = ReturnRunnedLevelId();
            if(currentLevel < levels.Count)
                RunLevel(currentLevel + 1);
        }
    }
}
