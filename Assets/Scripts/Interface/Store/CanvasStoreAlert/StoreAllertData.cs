using System;
using UnityEngine;

namespace Interface.Store.CanvasStoreAlert
{
    [Serializable]
    public class StoreAlertData
    {
        public enum Type
        {
            Success, Failed
        }

        public Type type;
        public string title;
        public string context;
        public AudioClip audio;
        public Sprite icon;
    }
}
