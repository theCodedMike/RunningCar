using System;
using ScriptableObjects.Settings;
using UnityEngine;

namespace Data
{
    public class SettingsHolder : MonoBehaviour
    {
        public static SettingsHolder Instance { get; private set; }

        public SettingsEditor settingsEditor;
        
        public SettingsCommon settingsCommon;
        
        public SettingsGame settingsGame;

        public SettingsAds settingsAds;

        public SettingsInApps settingsInApps;
        
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;
            } else 
                Destroy(gameObject);
        }
    }
}
