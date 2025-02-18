#if UNITY_EDITOR
using System.Collections.Generic;
using ScriptableObjects.Level;
using UnityEditor;
using UnityEngine;

namespace Interface.LevelEditor
{
    [ExecuteInEditMode]
    public class LevelEditorPreview : MonoBehaviour
    {
        public static LevelEditorPreview Instance { get; private set; }
        
        public LevelHolder levelHolder;
        public GameObject holder;

        public List<GameObject> holderObjects = new();
        public Level level;

        public bool isCurrentLevelLoaded = false;
        public int currentLevel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneClosing += SceneClosing;
        }

        public void Load(int levelId)
        {
            Clear();
            currentLevel = levelId;
            level = levelHolder.GetLevelById(currentLevel);
            isCurrentLevelLoaded = true;
            DrawObjects();
        }

        private void SceneClosing(UnityEngine.SceneManagement.Scene scene, bool removingScene)
        {
            Clear();
        }




        #region Interaction
        public void Save()
        {
            //Check is a same level
            if (holderObjects.Count > 0)
            {
                for (int i = 0; i < holderObjects.Count; i++)
                {
                    if (holderObjects[i] != null)
                    {
                        if (holderObjects[i].GetComponent<LevelEditorGameObject>().level != level)
                            return;
                    }
                }
            }

            //Clean objects in level
            level.objects = new List<LevelObject>();
            
            //Fill new objects
            for (int i = 0; i < holderObjects.Count; i++)
            {
                if (holderObjects[i] != null)
                {
                    Transform oldLevelObject = holderObjects[i].transform;
                    LevelObject newLevelObject = new LevelObject
                    {
                        prefab = oldLevelObject.GetComponent<LevelEditorGameObject>().levelObject.prefab,
                        visual = oldLevelObject.gameObject,
                        position = oldLevelObject.position,
                        eulerAngles = oldLevelObject.eulerAngles,
                        localScale = oldLevelObject.localScale
                    };

                    level.objects.Add(newLevelObject);
                }
            }
            
            print("Level editor: Saved");
        }

        public void Close()
        {
            Save();
            Clear();
        }
        #endregion




        #region View

        public void DrawObjects()
        {
            CleanHolder();
            foreach (var obj in level.objects)
                SetObject(obj);
        }

        private void SetObject(LevelObject levelObject)
        {
            if (levelObject.prefab != null)
            {
                GameObject newVisual = Instantiate(levelObject.prefab);
                newVisual.transform.parent = holder.transform;
                newVisual.transform.position = levelObject.position;
                newVisual.transform.eulerAngles = levelObject.eulerAngles;
                newVisual.transform.localScale = levelObject.localScale;
                newVisual.AddComponent<LevelEditorGameObject>();
                newVisual.GetComponent<LevelEditorGameObject>().Prepare(levelObject, level);
                
                holderObjects.Add(newVisual);
                levelObject.visual = newVisual;
            }
        }
        
        public void Clear()
        {
            isCurrentLevelLoaded = false;
            CleanHolder();
        }

        public void CleanHolder()
        {
            if (holder == null)
                return;
            if (holder.transform.childCount > 0)
            {
                for(int i = holder.transform.childCount - 1; i >= 0; i--) 
                    DestroyImmediate(holder.transform.GetChild(i).gameObject);
            }

            holderObjects = new();
        }
        public void SetActiveLatestObject()
        {
            if (level == null || level.objects == null || level.objects.Count == 0)
                return;

            Selection.objects = new Object[] { level.objects[^1].visual };
        }
        #endregion
    }
}
#endif