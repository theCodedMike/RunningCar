using Data;
using Game.Objects.Grounds;
using ScriptableObjects.Level;
using UnityEngine;

namespace Game
{
    public class LevelLoader : MonoBehaviour
    {
        public GameObject levelObjectHolder;
        public GameObject playerStart;
        [HideInInspector]
        public GameObject playerStartCurrent;
        public int groundCount = 0;
        
        private ObjectsHolder _objectsHolder;


        private void Awake()
        {
            _objectsHolder = GameObject.Find("ObjectsHolder").GetComponent<ObjectsHolder>();
        }

        public void Load()
        {
            Level level = _objectsHolder.levelHolder.ReturnRunnedLevel();
            foreach (LevelObject obj in level.objects)
                SetObject(obj);
            groundCount--;
        }

        private void SetObject(LevelObject levelObj)
        {
            if (levelObj.prefab == null)
                return;

            GameObject obj = Instantiate(levelObj.prefab, levelObjectHolder.transform);
            obj.transform.position = levelObj.position;
            obj.transform.eulerAngles = levelObj.eulerAngles;
            obj.transform.localScale = levelObj.localScale;

            Ground ground = obj.GetComponent<Ground>();
            if (obj.CompareTag("Ground") && ground)
                groundCount += ground.blockCount;
            if (levelObj.prefab == playerStart)
                playerStartCurrent = obj;
        }
    }
}
