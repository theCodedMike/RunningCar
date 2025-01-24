using System;
using UnityEngine;

namespace ScriptableObjects.GamePrefabs
{
    [Serializable]
    public class GamePrefab
    {
        public GameObject prefab;
        public Sprite icon;
        public string name;
    }
}
