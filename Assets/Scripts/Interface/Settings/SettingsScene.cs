using Data;
using UnityEngine;

namespace Interface.Settings
{
    public class SettingsScene : MonoBehaviour
    {
        private SettingsHolder _settingsHolder;
        public GameObject switchNotification;

        private void Start()
        {
            if (_settingsHolder == null) 
                _settingsHolder = SettingsHolder.Instance;
            TryToRemoveSwitchNotification();
        }

        private void TryToRemoveSwitchNotification()
        {
            if(!_settingsHolder.settingsCommon.notificationOn)
                Destroy(switchNotification.gameObject);
        }
        
        
    }
}
