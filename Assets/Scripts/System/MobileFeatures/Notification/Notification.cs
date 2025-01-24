using System.Utility;
using Data;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace System.MobileFeatures.Notification
{
    public class Notification : MonoBehaviour
    {
        public bool isActiveHere = true; // Make active only at launch scene

        private SettingsHolder _settingsHolder = SettingsHolder.Instance;

        private NotificationData[] _notificationData;

        #region Standard system methods

        // Start is called before the first frame update
        private void Start()
        {
            _settingsHolder = SettingsHolder.Instance;
            _notificationData = _settingsHolder.settingsCommon.notificationData;

            if (isActiveHere && _settingsHolder.settingsCommon.notificationOn)
                Create();
        }

        #endregion

        private void Create()
        {
            if (CustomPlayerPrefs.GetBool("notification"))
            {
                for (int i = 0; i < _notificationData.Length - 1; i++)
                {
                    NotificationData data = _notificationData[i];
                    ScheduleNotification("n" + i, data.title, data.GetRandomText(), data.delayHours);
                }
            } else 
                CancelAll();
        }

        private void ScheduleNotification(string id, string title, string text, int afterHours)
        {
#if UNITY_IOS
            CreateIOSNotification(id, title, text, afterHours);
#elif UNITY_ANDROID
            CreateAndroidNotification(id, title, text, afterHours);
#endif
        }

        private void CreateIOSNotification(string id, string title, string text, int afterHours)
        {
#if UNITY_IOS
            iOSNotificationCenter.RemoveScheduledNotification(id);

            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(afterHours, 0, 0),
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                Identifier = id,
                Title = title,
                Body = text,
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
#endif
        }

        private void CreateAndroidNotification(string id, string title, string text, int afterHours)
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
            var c = new AndroidNotificationChannel()
            {
                Id = id,
                Name = "Default Channel",
                Importance = Importance.High,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(c);
    
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = text;
            notification.FireTime = System.DateTime.Now.AddHours(afterHours);
    
            AndroidNotificationCenter.SendNotification(notification, id);
#endif
        }

        public void CancelAll()
        {
#if UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
#elif UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
#endif
        }
    }
}