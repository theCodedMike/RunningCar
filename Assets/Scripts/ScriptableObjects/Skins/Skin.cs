using System;
using System.Utility;
using UnityEngine;

namespace ScriptableObjects.Skins
{
    [Serializable]
    public class Skin
    {
        public string name = "";
        public string id = "";

        [Space()]
        public int price;
        public bool isFirst = false;

        [Space()]
        public Mesh mesh;
        public Material material;

        [Range(0, 1)]
        public float speed;
        [Range(0, 1)]
        public float steering;
        [Range(0, 1)]
        public float velocity;

        #region State

        public bool IsUnlocked() => CustomPlayerPrefs.GetBool("skinUnlocked_" + id, isFirst);
        
        public void Unlock() => CustomPlayerPrefs.SetBool("skinUnlocked_" + id, true);

        public void Lock() => CustomPlayerPrefs.SetBool("skinUnlocked_" + id, false);
        
        #endregion

        public bool Buy() => false;
    }
}
