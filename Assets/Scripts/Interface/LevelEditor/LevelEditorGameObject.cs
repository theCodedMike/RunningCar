#if UNITY_EDITOR
using ScriptableObjects.Level;
using UnityEngine;

namespace Interface.LevelEditor
{
    public class LevelEditorGameObject : MonoBehaviour
    {
        public LevelObject levelObject;
        public Level level;

        public void Prepare(LevelObject newLevelObject, Level newLevel)
        {
            levelObject = newLevelObject;
            level = newLevel;
        }
    }
}
#endif
