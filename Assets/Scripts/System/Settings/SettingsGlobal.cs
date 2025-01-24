using System.Utility;

namespace System.Settings
{
    public static class SettingsGlobal
    {
        public enum DeviceType
        {
            iPad, iPhoneClassic, iPhoneModern
        }

        public static DeviceType getCurrentDeviceType() => (DeviceType)CustomPlayerPrefs.GetInt("_deviceType");
    }
}
