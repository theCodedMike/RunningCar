using System.Utility;
using Data;
using UnityEngine;

namespace System.Settings
{
    public class LaunchSettings : MonoBehaviour
    {
        private SettingsHolder _settingsHolder;

        private void Start()
        {
            if (_settingsHolder == null)
                _settingsHolder = SettingsHolder.Instance;
            
            RecogniseDeviceType();
            SetTargetFrameRate();
        }


        private void RecogniseDeviceType()
        {
            float width = Screen.safeArea.width;
            float height = Screen.safeArea.height;
            float proportion = height > width ? height / width : width / height;
            
            if(proportion <= 1.5)
                CustomPlayerPrefs.SetInt("_deviceType", (int)SettingsGlobal.DeviceType.iPad);
            else if(proportion <= 1.8)
                CustomPlayerPrefs.SetInt("_deviceType", (int)SettingsGlobal.DeviceType.iPhoneClassic);
            else
                CustomPlayerPrefs.SetInt("_deviceType", (int)SettingsGlobal.DeviceType.iPhoneModern);
            
            CustomPlayerPrefs.Save();
        }

        private void SetTargetFrameRate()
        {
            Application.targetFrameRate = _settingsHolder.settingsCommon.targetFrameRate;
            QualitySettings.vSyncCount = 1;
        }
    }
}
