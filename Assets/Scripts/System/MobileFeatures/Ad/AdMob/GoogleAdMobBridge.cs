#if UNITY_EDITOR
#undef ADMOB_INTEGRATED
#if ADMOB_INTEGRATED
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif

namespace System.MobileFeatures.Ad.AdMob
{
    public static class GoogleAdMobBridge 
    {
        public static void ApplyAppId(string ios, string android)
        {
#if ADMOB_INTEGRATED
        GoogleMobileAds.Editor.GoogleMobileAdsSettings googleMobileAdsSettings = Resources.Load<GoogleMobileAds.Editor.GoogleMobileAdsSettings>("GoogleMobileAdsSettings");
        googleMobileAdsSettings.GoogleMobileAdsAndroidAppId = android;
        googleMobileAdsSettings.GoogleMobileAdsIOSAppId = ios;
#endif
        }
    }
}
#endif