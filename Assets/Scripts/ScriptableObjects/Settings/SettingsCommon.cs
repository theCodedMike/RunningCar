using System.MobileFeatures.Notification;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsCommon", menuName = "RunningCar/Settings/Common", order = 25)]
    public class SettingsCommon : ScriptableObject
    {
        [Header("Basic")]
        public int targetFrameRate = 300;
        
        [Header("IOS")]
        public bool gameUseEncryption = false;
        
        [Header("RateUs")]
        public string rateUsIosAppId = "";
        
        public string rateUsAndroidPackageName = "";

        [Space(5)]
        [Header("Notifications")]
        public bool notificationOn = true;

        public NotificationData[] notificationData;
    }
}
