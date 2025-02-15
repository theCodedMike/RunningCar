#undef UNITYADS_INTEGRATED
using Data;
using UnityEngine;

#if UNITYADS_INTEGRATED
#if !UNITY_STANDALONE && !UNITY_WEBGL
using UnityEngine.Advertisements;
#endif
#endif


namespace System.MobileFeatures.Ad.UnityAd
{
#if UNITYADS_INTEGRATED
    public class UnityAdController : MonoBehaviour
#if !UNITY_STANDALONE && !UNITY_WEBGL
        , IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
#endif
    {
#else
    public class UnityAdController : MonoBehaviour
    {
#endif
        public static UnityAdController Instance { get; private set; }

        private UnityAdModel _model;

        #region Standard system methods
        private void Awake()
        { 
            if (Instance == null) 
                Instance = this;
        }
        private void Start()
        {
#if !UNITY_STANDALONE
            //Singleton object return
            if (Instance == null)
            {
                DontDestroyOnLoad(this);

                //Prepare
                _model = GetComponent<UnityAdModel>();
                _model.settingsHolder = SettingsHolder.Instance;
                _model.settingsAds = _model.settingsHolder.settingsAds;

#if UNITYADS_INTEGRATED
                if (!Advertisement.isInitialized & Advertisement.isSupported)
                {
#if UNITY_IOS
                    var appID = _model.settingsAds.unityAdsAppIdIOS;
                    _model.idBanner = _model.settingsAds.unityAdsBannerIdIOS;
                    _model.idInterstitial = _model.settingsAds.unityAdsInterstitialIdIOS;
                    _model.idRewarded = _model.settingsAds.unityAdsRewardedVideoIdIOS;
#elif UNITY_ANDROID || UNITY_EDITOR
                    var appID = _model.settingsAds.unityAdsAppIdAndroid;
                    _model.idBanner = _model.settingsAds.unityAdsBannerIdAndroid;
                    _model.idInterstitial = _model.settingsAds.unityAdsInterstitialIdAndroid;
                    _model.idRewarded = _model.settingsAds.unityAdsRewardedVideoIdAndroid;
#endif
                    Advertisement.Initialize(appID, _model.settingsAds.unityAdsTestMode, this);
                }
#endif
            }
            else
                Destroy(gameObject);
#endif
        }

        public bool IsIntegrated()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if UNITYADS_INTEGRATED
            return true;
#endif
#endif
            return false;
        }

#if !UNITY_STANDALONE && !UNITY_WEBGL
#if UNITYADS_INTEGRATED
        public void OnInitializationComplete()
        {
            Advertisement.Banner.SetPosition(_model.bannerPosition);

            LoadBanner();
            LoadInterstitial();
            LoadRewarded();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {}
#endif
#endif
        #endregion

        
        #region Banner
#if UNITYADS_INTEGRATED
        private void LoadBanner()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            Advertisement.Banner.Load(_model.idBanner, options);
#endif
        }
#endif

        public void ShowBanner()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if UNITYADS_INTEGRATED
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_model.idBanner, options);
#endif
#endif
        }

        public void HideBanner()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if UNITYADS_INTEGRATED
            Advertisement.Banner.Hide();
#endif
#endif
        }

#if UNITYADS_INTEGRATED
        void OnBannerLoaded() { }
        void OnBannerClicked() { }
        void OnBannerShown() { }
        void OnBannerHidden() { }

        // Implement code to execute when the load errorCallback event triggers:
        void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
            // Optionally execute additional code, such as attempting to load another ad.
        }
#endif
        #endregion

        
        #region Interstitial
#if UNITYADS_INTEGRATED
        private void LoadInterstitial()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            Advertisement.Load(_model.idInterstitial, this);
#endif
        }
#endif

        public void ShowInterstitial()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if UNITYADS_INTEGRATED
            Advertisement.Show(_model.idInterstitial, this);
#endif
#endif
        }

#if UNITYADS_INTEGRATED
#if !UNITY_STANDALONE && !UNITY_WEBGL
        public void OnUnityAdsShowStart(string placementId) {}
        public void OnUnityAdsShowClick(string placementId) {}
        public void OnUnityAdsAdLoaded(string placementId) {}

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == _model.idRewarded && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                if (GameObject.Find("Ad") != null)
                    GameObject.Find("Ad").GetComponent<AdsInterface>().GetReward(_model.rewardType);
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            if (placementId == _model.idInterstitial) LoadInterstitial();
            if (placementId == _model.idRewarded) LoadRewarded();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            if (placementId == _model.idInterstitial) LoadInterstitial();
            if (placementId == _model.idRewarded) LoadRewarded();
        }
#endif
#endif
        #endregion

        
        #region Rewarded
#if UNITYADS_INTEGRATED
        private void LoadRewarded()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            Advertisement.Load(_model.idRewarded, this);
#endif
        }
#endif

        public void ShowRewarded(AdRewardType type)
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if UNITYADS_INTEGRATED
            _model.rewardType = type;
            Advertisement.Show(_model.idRewarded, this);
#endif
#endif
        }
        #endregion
    }
}
