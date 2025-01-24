#undef ADMOB_INTEGRATED
using Data;
using ScriptableObjects.Settings;
using UnityEngine;

#if ADMOB_INTEGRATED
#if !UNITY_STANDALONE && !UNITY_WEBGL
using GoogleMobileAds.Api;      
#endif
#endif

namespace System.MobileFeatures.Ad.AdMob
{
    public class AdMobModel : MonoBehaviour
    {
        [HideInInspector]
        public SettingsHolder settingsHolder = SettingsHolder.Instance;
        [HideInInspector]
        public SettingsAds settingsAds;

#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
        [HideInInspector]
        public BannerView bannerView;
        [HideInInspector]
        public bool isLoadedBanner = false;
        [HideInInspector]
        public InterstitialAd interstitial;
        [HideInInspector]
        public RewardedAd rewardedAd;
#endif
#endif
    }
}
