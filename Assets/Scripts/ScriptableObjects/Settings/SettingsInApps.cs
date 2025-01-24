using System.InApp;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsInApps", menuName = "RunningCar/Settings/InApps", order = 25)]
    public class SettingsInApps : ScriptableObject
    {
        public bool inAppsOn = false;
        public bool inAppsDebugMessagesOn = false;

        public InApp[] inAppsBasic;
        public InApp[] inAppCoins;

        public InApp GetInAppForID(string id, InApp[] inApps) => inApps.FirstOrDefault(inApp => inApp.id == id);
    }
}
