using System.Linq;
using System.MobileFeatures.Ad;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsAds", menuName = "RunningCar/Settings/Ads", order = 25)]
    public class SettingsAds : ScriptableObject
    {
        [Header("adMob是否启用")]
        public bool adMobOn = false;

        [Space(5)]
        public string adMobAppIdIOS = "";
        public string adMobAppIdAndroid = "";

        [Space(10)]
        public string adMobBannerIdIOS = "";
        public string adMobBannerIdAndroid = "";
        
        [Space(5)]
        public string adMobInterstitialIdIOS = "";
        public string adMobInterstitialIdAndroid = "";
        
        [Space(5)]
        public string adMobRewardedVideoIdIOS = "";
        public string adMobRewardedVideoIdAndroid = "";

        [Header("unityAds是否启用")]
        public bool unityAdsOn = false;
        public bool unityAdsTestMode = true;
        
        [Space(5)]
        public string unityAdsAppIdIOS = "";
        public string unityAdsAppIdAndroid = "";

        [Space(10)]
        public string unityAdsBannerIdIOS = "";
        public string unityAdsBannerIdAndroid = "";
        
        [Space(5)]
        public string unityAdsInterstitialIdIOS = "";
        public string unityAdsInterstitialIdAndroid = "";
        
        [Space(5)]
        public string unityAdsRewardedVideoIdIOS = "";
        public string unityAdsRewardedVideoIdAndroid = "";

        [Header("Scenarios")]
        public AdShowOption[] showOptions;

        public bool showRewardVideoWhenLose = true;
        public bool showDoubleCoinsAtDailyReward = true;
        public bool showDoubleCoinsAtDailyTask = true;

        [Header("Debug")]
        public bool debugMessagesOn = true;

        public AdShowOption GetOptionForScene(string scene) => showOptions.FirstOrDefault(opt => opt.GetSceneString() == scene);
    }
}
