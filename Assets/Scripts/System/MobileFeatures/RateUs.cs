using Data;
using ScriptableObjects.Settings;
using UnityEngine;

namespace System.MobileFeatures
{
    public class RateUs : MonoBehaviour
    {
        private SettingsHolder _settingsHolder;
        private SettingsCommon _settingsCommon;

        private void Start()
        {
            if(_settingsHolder == null)
                _settingsHolder = SettingsHolder.Instance;
            if (_settingsCommon == null)
                _settingsCommon = _settingsHolder.settingsCommon;
        }

        public void Open()
        {
#if UNITY_IOS
            Application.OpenURL("itms-apps://itunes.apple.com/app/" + settingsCommon.rateUsIosAppId + "?action=write-review");
#else
            Application.OpenURL("market://details?id=" + _settingsCommon.rateUsAndroidPackageName);
#endif
        }
    }
}
