/*
#undef UNITYADS_INTEGRATED
using Data;
using ScriptableObjects.Settings;
using UnityEngine;

#if !UNITY_STANDALONE && !UNITY_WEBGL
using UnityEngine.Advertisements;
#endif

namespace System.MobileFeatures.Ad.UnityAd
{
    public class UnityAdModel : MonoBehaviour
    {
        [HideInInspector]
        public SettingsHolder settingsHolder = SettingsHolder.Instance;
        [HideInInspector]
        public SettingsAds settingsAds;
        
#if UNITYADS_INTEGRATED
#if !UNITY_STANDALONE && !UNITY_WEBGL
        [HideInInspector]
        public BannerPosition BannerPosition = BannerPosition.BOTTOM_CENTER;
#endif
#endif

        [HideInInspector]
        public string idBanner;
        [HideInInspector]
        public string idInterstitial;
        [HideInInspector]
        public string idRewarded;
        [HideInInspector]
        public AdRewardType rewardType;
    }
}
*/