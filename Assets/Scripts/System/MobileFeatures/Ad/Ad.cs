/*
using System.MobileFeatures.Ad.AdMob;
using System.MobileFeatures.Ad.UnityAd;
using System.Utility;
using Data;
using Game.Game;
using Interface.DailyReward;
using Interface.DailyTask;
using ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.MobileFeatures.Ad
{
    public enum AdRewardType
    {
        RewardAfterLose,
        DailyReward,
        DailyTask
    }

    public enum AdSystem
    {
        AdMob, UnityAds, Null
    }

    public enum AdType
    {
        Banner, Interstitial, Reward
    }
    public class AdsInterface : MonoBehaviour
    {
        private SettingsHolder _settingsHolder = SettingsHolder.Instance;
        private SettingsAds _settingsAds;

        private AdMobController _adMobController;
        private UnityAdController _unityAdController;

        private bool _isAvailableAdMob = false;
        private bool _isAvailableUnityAds = false;


        #region Standard system methods
        private void Start()
        {
            _settingsHolder = SettingsHolder.Instance;
            _settingsAds = _settingsHolder.settingsAds;
            _adMobController = AdMobController.Instance;
            _unityAdController = UnityAdController.Instance;
            
            HideBanner();
            
            CheckIsAvailable();
            RunShowAtStartScenario();
        }
        #endregion


        #region Status
        private void CheckIsAvailable()
        {
            if (_settingsAds.adMobOn)
            {
                if (_adMobController != null && _adMobController.IsIntegrated())
                    _isAvailableAdMob = true;
                else
                {
                    if (_settingsAds.debugMessagesOn)
                        print("AdMob: Is on, but not integrated. Check Settings Common windows.");
                }
            }
            else
            {
                if (_settingsAds.debugMessagesOn)
                    print("AdMob: Is off in Settings Common");
            }

            if (_settingsAds.unityAdsOn)
            {
                if (_unityAdController != null && _unityAdController.IsIntegrated())
                    _isAvailableUnityAds = true;
                else
                {
                    if (_settingsAds.debugMessagesOn)
                        print("Unity Ads: Is on, but not integrated. Check Settings Common windows.");
                }
            }
            else
            {
                if (_settingsAds.debugMessagesOn)
                    print("Unity Ads: Is off in Settings Common");
            }
        }

        private AdSystem GetAdSystemToShow()
        {
            if (_isAvailableAdMob && _isAvailableUnityAds)
            {
                bool showAdMob = CustomPlayerPrefs.GetBool("ads_showAdMobNow");
                CustomPlayerPrefs.SetBool("ads_showAdMobNow", !showAdMob);
                return showAdMob ? AdSystem.AdMob : AdSystem.UnityAds;
            }

            if (_isAvailableAdMob)
                return AdSystem.AdMob;
            if(_isAvailableUnityAds)
                return AdSystem.UnityAds;
            return AdSystem.Null;
        }
        #endregion


        #region Scenarios
        private void RunShowAtStartScenario()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            AdShowOption option = _settingsAds.GetOptionForScene(sceneName);
            if (option != null)
            {
                if(option.bannerShowAtStart)
                    ShowBanner();
                if(option.interstitialShowAtStart)
                    ShowInterstitial();
            }
        }
        #endregion


        #region AdShow
        public void ShowBanner()
        {
            if (!CustomPlayerPrefs.GetBool("ads_removedByInApp", false))
            {
                AdSystem system = GetAdSystemToShow();
                switch (system)
                {
                    case AdSystem.AdMob: _adMobController.ShowBanner(); break;
                    case AdSystem.UnityAds: _unityAdController.ShowBanner(); break;
                    default: Debug.LogWarning("adSystem is null..."); break;
                }
            }
        }

        public void HideBanner()
        {
            if(_adMobController != null)
                _adMobController.HideBanner();
            if(_unityAdController != null)
                _unityAdController.HideBanner();
        }

        public void ShowInterstitial()
        {
            if (!CustomPlayerPrefs.GetBool("ads_removedByInApp", false))
            {
                AdSystem system = GetAdSystemToShow();
                switch (system)
                {
                    case AdSystem.AdMob: _adMobController.ShowInterstitial(); break;
                    case AdSystem.UnityAds: _unityAdController.ShowInterstitial(); break;
                    default: Debug.LogWarning("adSystem is null..."); break;
                }
            }
        }

        public void ShowReward(AdRewardType type)
        {
            AdSystem system = GetAdSystemToShow();
            switch (system)
            {
                case AdSystem.AdMob: _adMobController.ShowRewardedAd(type); break;
                case AdSystem.UnityAds: _unityAdController.ShowRewarded(type); break;
                default: Debug.LogWarning("adSystem is null..."); break;
            }
        }
        #endregion


        #region Rewards

        public void GetReward(AdRewardType type)
        {
            switch (type)
            {
                case AdRewardType.RewardAfterLose:
                    GameObject game = GameObject.Find("Game");
                    if (game != null)
                        game.GetComponent<GameController>().ContinueGame();
                    break;
                case AdRewardType.DailyReward:
                    GameObject rewardScene = GameObject.Find("DailyRewardScene");
                    if (rewardScene != null)
                        rewardScene.GetComponent<DailyRewardScene>().ReceiveDoubleReward();
                    break;
                case AdRewardType.DailyTask:
                    GameObject taskScene = GameObject.Find("DailyTaskScene");
                    if (taskScene != null)
                        taskScene.GetComponent<DailyTaskScene>().ReceiveDoubleReward();
                    break;
            }
        }
        #endregion
    }
}
*/