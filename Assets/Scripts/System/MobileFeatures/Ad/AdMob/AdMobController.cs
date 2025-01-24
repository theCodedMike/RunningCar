#undef ADMOB_INTEGRATED
using UnityEngine;

#if ADMOB_INTEGRATED
#if !UNITY_STANDALONE && !UNITY_WEBGL
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif
#endif


namespace System.MobileFeatures.Ad.AdMob
{
    public class AdMobController : MonoBehaviour
    {
        public static AdMobController Instance { get; private set; }

        private AdMobModel _model;

        #region Standard system methods

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
            if(Instance == null) 
            {
                DontDestroyOnLoad(this);

                _model = GetComponent<AdMobModel>();
                _model.settingsHolder = SettingsHolder.Instance;
                _model.settingsAds = _model.settingsHolder.settingsAds;

#if ADMOB_INTEGRATED
                MobileAds.Initialize(initStatus => { });

                RequestBanner();
                RequestInterstitial();
                RequestRewardedAd();
#endif
            }
            else
                Destroy(gameObject);
#endif
        }

        public bool IsIntegrated()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
            return true;
#endif
#endif
            return false;
        }
        #endregion


        #region Banner
        private void RequestBanner()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
#if UNITY_IOS
            string adUnitId = _model.settingsAds.adMobBannerIdIOS;
#elif UNITY_ANDROID || UNITY_EDITOR
            string adUnitId = _model.settingsAds.adMobBannerIdAndroid;
#endif
            // 销毁旧的banner
            if (_model.bannerView != null)
                _model.bannerView.Destroy();
            // 在屏幕下方创建新的
            _model.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
            _model.bannerView.OnBannerAdLoaded += () =>
            {
                _model.isLoadedBanner = true;
            };
            _model.bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                _model.isLoadedBanner = false;
            };
            _model.bannerView.OnAdFullScreenContentClosed += () =>
            {
                _model.isLoadedBanner = false;
                RequestBanner();
            };
            _model.bannerView.OnAdFullScreenContentOpened += () =>
            {
                if (GameObject.Find("BANNER(Clone)") != null)
                    GameObject.Find("BANNER(Clone)").GetComponent<Canvas>().sortingOrder = 99998;
            };
            _model.bannerView.LoadAd(new AdRequest());
            _model.bannerView.Hide();
#endif   
#endif
        }
        
        public void ShowBanner()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
            if (_model.isLoadedBanner)
                _model.bannerView.Show();
            else
                RequestBanner();
#endif
#endif
        }

        public void HideBanner()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
            if (_model.bannerView != null)
                _model.bannerView.Destroy();
            RequestBanner();
#endif
#endif
        }
        #endregion


        #region Interstitial
        public void RequestInterstitial()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
#if UNITY_IOS
            var adUnitId = _model.settingsAds.adMobInterstitialIdIOS;
#elif UNITY_ANDROID || UNITY_EDITOR
            var adUnitId = _model.settingsAds.adMobInterstitialIdAndroid;
#endif
            if (_model.interstitial != null)
            {
                _model.interstitial.Destroy();
                _model.interstitial = null;
            }

            // Initialize an InterstitialAd.
            InterstitialAd.Load(adUnitId, new AdRequest(), (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    RequestInterstitial();
                    return;
                }

                _model.interstitial = ad;
                _model.interstitial.OnAdFullScreenContentOpened += () =>
                {
                    if (GameObject.Find("768x1024(Clone)") != null)
                        GameObject.Find("768x1024(Clone)").GetComponent<Canvas>().sortingOrder = 2000;
                };
                _model.interstitial.OnAdFullScreenContentClosed += () =>
                {
                    RequestInterstitial();
                };
                _model.interstitial.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    RequestInterstitial();
                };
            });
#endif
#endif
        }

        public void ShowInterstitial()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
            if (_model.interstitial.CanShowAd())
                _model.interstitial.Show();
            else
                RequestInterstitial();
#endif
#endif
        }
        #endregion
        
        
        #region RewardedAds
        public void RequestRewardedAd()
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
#if UNITY_IOS
            var adUnitId = _model.settingsAds.adMobRewardedVideoIdIOS;
#elif UNITY_ANDROID || UNITY_EDITOR
            var adUnitId = _model.settingsAds.adMobRewardedVideoIdAndroid;
#endif
            if (_model.rewardedAd != null)
            {
                _model.rewardedAd.Destroy();
                _model.rewardedAd = null;
            }

            RewardedAd.Load(adUnitId, new AdRequest(), (RewardedAd ad, LoadAdError error) => {
                if (error != null || ad == null) {
                    RequestRewardedAd();
                    return;
                }

                _model.rewardedAd = ad;
                _model.rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    RequestRewardedAd();
                };
                _model.rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    RequestRewardedAd();
                };
            });
#endif
#endif
        }

        public void ShowRewardedAd(AdRewardType type)
        {
#if !UNITY_STANDALONE && !UNITY_WEBGL
#if ADMOB_INTEGRATED
            if (_model.rewardedAd != null && _model.rewardedAd.CanShowAd())
            {
                _model.rewardedAd.Show((Reward reward) =>
                {
                    if (GameObject.Find("Ad") != null)
                        GameObject.Find("Ad").GetComponent<AdsInterface>().GetReward(type);
                });
            }
            else
            {
                RequestRewardedAd();
            }
#endif
#endif
        }
        #endregion
    }
}
