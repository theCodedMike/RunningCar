using System;
using UnityEngine;

namespace ScriptableObjects.Level
{
    [Serializable]
    public class LevelObject
    {
        public GameObject prefab;
        public GameObject visual;

        public Vector3 position;
        public Vector3 eulerAngles;
        public Vector3 localScale = Vector3.one;
    }
}
