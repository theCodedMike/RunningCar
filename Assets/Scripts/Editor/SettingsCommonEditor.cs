#if UNITY_EDITOR
//using System.MobileFeatures.Ad.AdMob;
//using System.MobileFeatures.Ad.UnityAd;
using Editor.Components;
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SettingsCommonEditor : EditorWindow
    {
        //Components
        private int _toolbarMainSelected;
        private int _toolbarAdsSelected;
        private readonly string[] _toolbarMain = { "General", "Ads", "In-Apps", "Notifications", "Audio" };
        private readonly string[] _toolbarAds = { "AdMob", "Unity Ads", "Show Options" };

        private bool _isAdsShownOptionsFolded = true;
        private bool _isInAppBasicFolded = true;
        private bool _isInAppCoinsFolded;

        private Vector2 _scrollPosition;

        private static SettingsEditor _settingsEditor;
        private static SerializedObject _soSettingsCommon;
        private const string AssetAddressSettingsEditor = "Assets/ScriptableObjects/Settings/SettingsEditor.asset";
        private static SerializedObject _soSettingsAds;
        private static SerializedObject _soSettingsInApps;
        private static SerializedObject _soSettingsAudio;

        //Values
        private static SerializedProperty _targetFrameRate;
        private static SerializedProperty _gameUseEncryption;
        private static SerializedProperty _rateUsIosAppId;
        private static SerializedProperty _rateUsAndroidPackageName;

        //Ads
        private static SerializedProperty _adMobOn;
        private static SerializedProperty _adMobAppIdIOS;
        private static SerializedProperty _adMobAppIdAndroid;
        private static SerializedProperty _adMobBannerIdIOS;
        private static SerializedProperty _adMobBannerIdAndroid;
        private static SerializedProperty _adMobInterstitialIdIOS;
        private static SerializedProperty _adMobInterstitialIdAndroid;
        private static SerializedProperty _adMobRewardedVideoIdIOS;
        private static SerializedProperty _adMobRewardedVideoIdAndroid;

        private static SerializedProperty _unityAdsOn;
        private static SerializedProperty _unityAdsAppIdIOS;
        private static SerializedProperty _unityAdsAppIdAndroid;
        private static SerializedProperty _unityAdsBannerIdIOS;
        private static SerializedProperty _unityAdsBannerIdAndroid;
        private static SerializedProperty _unityAdsInterstitialIdIOS;
        private static SerializedProperty _unityAdsInterstitialIdAndroid;
        private static SerializedProperty _unityAdsRewardedVideoIdIOS;
        private static SerializedProperty _unityAdsRewardedVideoIdAndroid;

        private static SerializedProperty _shownOptions;

        private static SerializedProperty _showRewardVideoWhenLose;
        private static SerializedProperty _showDoubleCoinsAtDailyReward;
        private static SerializedProperty _showDoubleCoinsAtDailyTask;

        private static SerializedProperty _debugMessagesOn;

        //InApps
        private static SerializedProperty _inAppsOn;
        private static SerializedProperty _inAppsDebugMessagesOn;

        private static SerializedProperty _inAppsBasic;
        private static SerializedProperty _inAppCoins;

        //Notification
        private static SerializedProperty _notificationOn;
        private static SerializedProperty _notificationDataProperty;

        //Audio
        private static SerializedProperty _audioButtonClick;
        private static SerializedProperty _audioSwitch;
        private static SerializedProperty _audioError;
        private static SerializedProperty _audioSuccessful;
        private static SerializedProperty _audioEquip;

        private static SerializedProperty _audioDailyRewardWheel;
        private static SerializedProperty _audioDailyTaskNotification;

        private static SerializedProperty _audioCoin;
        private static SerializedProperty _audioPoint;
        private static SerializedProperty _audioBrake;
        private static SerializedProperty _audioJump;
        private static SerializedProperty _audioTurn;
        private static SerializedProperty _audioSaw;
        private static SerializedProperty _audioEngine;
        private static SerializedProperty _audioSpeedUp;
        private static SerializedProperty _audioSlowDown;

        private static SerializedProperty _audioSmash1;
        private static SerializedProperty _audioSmash2;

        private static SerializedProperty _audioStartLinePrepare;
        private static SerializedProperty _audioStartLineRun;
        private static SerializedProperty _audioLose;
        private static SerializedProperty _audioWin;

        private static SerializedProperty _musicGame;
        private static SerializedProperty _musicMenu;

        [MenuItem("Tools/RunningCar/Settings Common", false, 41)]
        public static void ShowWindow()
        {
            GetWindow<SettingsCommonEditor>("RunningCar - Settings Common");
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(AssetAddressSettingsEditor);

            _soSettingsCommon = new SerializedObject(_settingsEditor.settingsCommon);
            _soSettingsAds = new SerializedObject(_settingsEditor.settingsAds);
            _soSettingsInApps = new SerializedObject(_settingsEditor.settingsInApps);
            _soSettingsAudio = new SerializedObject(_settingsEditor.settingsAudio);
            _soSettingsCommon.Update();
            _soSettingsAds.Update();
            _soSettingsInApps.Update();
            _soSettingsAudio.Update();

            //Common
            _targetFrameRate = _soSettingsCommon.FindProperty("targetFrameRate");
            _gameUseEncryption = _soSettingsCommon.FindProperty("gameUseEncryption");
            _rateUsIosAppId = _soSettingsCommon.FindProperty("rateUsIosAppId");
            _rateUsAndroidPackageName = _soSettingsCommon.FindProperty("rateUsAndroidPackageName");

            //Ads
            _adMobOn = _soSettingsAds.FindProperty("adMobOn");
            _adMobAppIdIOS = _soSettingsAds.FindProperty("adMobAppIdIOS");
            _adMobAppIdAndroid = _soSettingsAds.FindProperty("adMobAppIdAndroid");
            _adMobBannerIdIOS = _soSettingsAds.FindProperty("adMobBannerIdIOS");
            _adMobBannerIdAndroid = _soSettingsAds.FindProperty("adMobBannerIdAndroid");
            _adMobInterstitialIdIOS = _soSettingsAds.FindProperty("adMobInterstitialIdIOS");
            _adMobInterstitialIdAndroid = _soSettingsAds.FindProperty("adMobInterstitialIdAndroid");
            _adMobRewardedVideoIdIOS = _soSettingsAds.FindProperty("adMobRewardedVideoIdIOS");
            _adMobRewardedVideoIdAndroid = _soSettingsAds.FindProperty("adMobRewardedVideoIdAndroid");

            _unityAdsOn = _soSettingsAds.FindProperty("unityAdsOn");
            _unityAdsAppIdIOS = _soSettingsAds.FindProperty("unityAdsAppIdIOS");
            _unityAdsAppIdAndroid = _soSettingsAds.FindProperty("unityAdsAppIdAndroid");
            _unityAdsBannerIdIOS = _soSettingsAds.FindProperty("unityAdsBannerIdIOS");
            _unityAdsBannerIdAndroid = _soSettingsAds.FindProperty("unityAdsBannerIdAndroid");
            _unityAdsInterstitialIdIOS = _soSettingsAds.FindProperty("unityAdsInterstitialIdIOS");
            _unityAdsInterstitialIdAndroid = _soSettingsAds.FindProperty("unityAdsInterstitialIdAndroid");
            _unityAdsRewardedVideoIdIOS = _soSettingsAds.FindProperty("unityAdsRewardedVideoIdIOS");
            _unityAdsRewardedVideoIdAndroid = _soSettingsAds.FindProperty("unityAdsRewardedVideoIdAndroid");

            _shownOptions = _soSettingsAds.FindProperty("shownOptions");

            _showRewardVideoWhenLose = _soSettingsAds.FindProperty("showRewardVideoWhenLose");
            _showDoubleCoinsAtDailyReward = _soSettingsAds.FindProperty("showDoubleCoinsAtDailyReward");
            _showDoubleCoinsAtDailyTask = _soSettingsAds.FindProperty("showDoubleCoinsAtDailyTask");

            _debugMessagesOn = _soSettingsAds.FindProperty("debugMessagesOn");

            //InApps
            _inAppsOn = _soSettingsInApps.FindProperty("inAppsOn");

            _inAppsDebugMessagesOn = _soSettingsInApps.FindProperty("inAppsDebugMessagesOn");

            _inAppsBasic = _soSettingsInApps.FindProperty("inAppsBasic");
            _inAppCoins = _soSettingsInApps.FindProperty("inAppCoins");

            //Notification
            _notificationOn = _soSettingsCommon.FindProperty("notificationOn");
            _notificationDataProperty = _soSettingsCommon.FindProperty("notificationData");

            //Audio
            _audioButtonClick = _soSettingsAudio.FindProperty("audioButtonClick");
            _audioSwitch = _soSettingsAudio.FindProperty("audioSwitch");
            _audioError = _soSettingsAudio.FindProperty("audioError");
            _audioSuccessful = _soSettingsAudio.FindProperty("audioSuccessful");
            _audioEquip = _soSettingsAudio.FindProperty("audioEquip");

            _audioDailyRewardWheel = _soSettingsAudio.FindProperty("audioDailyRewardWheel");
            _audioDailyTaskNotification = _soSettingsAudio.FindProperty("audioDailyTaskNotification");

            _audioCoin = _soSettingsAudio.FindProperty("audioCoin");
            _audioPoint = _soSettingsAudio.FindProperty("audioPoint");
            _audioBrake = _soSettingsAudio.FindProperty("audioBrake");
            _audioJump = _soSettingsAudio.FindProperty("audioJump");
            _audioTurn = _soSettingsAudio.FindProperty("audioTurn");
            _audioSaw = _soSettingsAudio.FindProperty("audioSaw");
            _audioEngine = _soSettingsAudio.FindProperty("audioEngine");
            _audioSpeedUp = _soSettingsAudio.FindProperty("audioSpeedUp");
            _audioSlowDown = _soSettingsAudio.FindProperty("audioSlowDown");

            _audioSmash1 = _soSettingsAudio.FindProperty("audioSmash1");
            _audioSmash2 = _soSettingsAudio.FindProperty("audioSmash2");

            _audioStartLinePrepare = _soSettingsAudio.FindProperty("audioStartLinePrepare");
            _audioStartLineRun = _soSettingsAudio.FindProperty("audioStartLineRun");
            _audioLose = _soSettingsAudio.FindProperty("audioLose");
            _audioWin = _soSettingsAudio.FindProperty("audioWin");

            _musicGame = _soSettingsAudio.FindProperty("musicGame");
            _musicMenu = _soSettingsAudio.FindProperty("musicMenu");
        }

        void OnGUI()
        {
            _soSettingsCommon.Update();
            _soSettingsAds.Update();
            _soSettingsInApps.Update();
            _soSettingsAudio.Update();

            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, CustomGUIStyle.Box(_settingsEditor.backgroundColor));

            _toolbarMainSelected = GUILayout.Toolbar(_toolbarMainSelected, _toolbarMain, _settingsEditor.toolbarMain, CustomGUILayout.ToolBar());
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.Space(10);
            switch (_toolbarMainSelected)
            {
                case 0:
                    DrawGeneral();
                    break;
                case 1:
                    DrawAds();
                    break;
                case 2:
                    DrawIAP();
                    break;
                case 3:
                    DrawNotification();
                    break;
                default:
                    DrawAudio();
                    break;
            }
            EditorGUILayout.EndScrollView();

            _soSettingsCommon.ApplyModifiedProperties();
            _soSettingsAds.ApplyModifiedProperties();
            _soSettingsInApps.ApplyModifiedProperties();
            _soSettingsAudio.ApplyModifiedProperties();
        }

        #region Window

        private void DrawGeneral()
        {
            GUILayout.Label("General", _settingsEditor.labelTitle);

            _targetFrameRate.intValue = EditorGUILayout.IntField("Target FPS:", _targetFrameRate.intValue);
            GUILayout.Label("iOS", _settingsEditor.labelParameter);
            _gameUseEncryption.boolValue = EditorGUILayout.Toggle("Use encryption:", _gameUseEncryption.boolValue);

            //RateUs
            EditorGUILayout.Space(10);
            GUILayout.Label("RateUs", _settingsEditor.labelTitle);
            _rateUsIosAppId.stringValue = EditorGUILayout.TextField("iOS App ID:", _rateUsIosAppId.stringValue);
            _rateUsAndroidPackageName.stringValue = EditorGUILayout.TextField("Android Package Name:", _rateUsAndroidPackageName.stringValue);
        }

        #region Ads

        private void DrawAds()
        {
            _toolbarAdsSelected = GUILayout.Toolbar(_toolbarAdsSelected, _toolbarAds, _settingsEditor.toolbarSmall, CustomGUILayout.ToolbarSmall());
            switch (_toolbarAdsSelected)
            {
                case 0:
                    DrawAdsAdMob();
                    break;
                case 1:
                    DrawAdsUnityAds();
                    break;
                default:
                    DrawAdsShowOptions();
                    break;
            }
        }

        private void DrawAdsAdMob()
        {
            GUILayout.Label("AdMob", _settingsEditor.labelTitle);

            //Values
            _adMobOn.boolValue = EditorGUILayout.Toggle("On: ", _adMobOn.boolValue);
            EditorGUILayout.Space(8);

            GUILayout.Label("AppId:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _adMobAppIdIOS.stringValue = EditorGUILayout.TextArea(_adMobAppIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _adMobAppIdAndroid.stringValue = EditorGUILayout.TextArea(_adMobAppIdAndroid.stringValue);

            EditorGUILayout.Space(12);
            GUILayout.Label("Banner:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _adMobBannerIdIOS.stringValue = EditorGUILayout.TextArea(_adMobBannerIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _adMobBannerIdAndroid.stringValue = EditorGUILayout.TextArea(_adMobBannerIdAndroid.stringValue);

            EditorGUILayout.Space(4);
            GUILayout.Label("Interstitial:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _adMobInterstitialIdIOS.stringValue = EditorGUILayout.TextArea(_adMobInterstitialIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _adMobInterstitialIdAndroid.stringValue = EditorGUILayout.TextArea(_adMobInterstitialIdAndroid.stringValue);

            EditorGUILayout.Space(4);
            GUILayout.Label("Interstitial:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _adMobRewardedVideoIdIOS.stringValue = EditorGUILayout.TextArea(_adMobRewardedVideoIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _adMobRewardedVideoIdAndroid.stringValue = EditorGUILayout.TextArea(_adMobRewardedVideoIdAndroid.stringValue);

            //Buttons
            EditorGUILayout.Space(10);
            GUILayout.Label("Interaction", _settingsEditor.labelParameter);
            /*
            if (GUILayout.Button("Apply AppID", _settingsEditor.buttonGreen)) 
                GoogleAdMobBridge.ApplyAppId(_adMobAppIdIOS.stringValue, _adMobAppIdAndroid.stringValue);*/
            EditorGUILayout.BeginHorizontal();
            /*
            if (GUILayout.Button("Integrate", _settingsEditor.buttonYellow)) 
                GoogleAdMobIntegrator.Integrate();
            if (GUILayout.Button("Disintegrate", _settingsEditor.buttonRed)) 
                GoogleAdMobIntegrator.Disintegrate();*/
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            GUILayout.Label("Helps", _settingsEditor.labelParameter);
            if (GUILayout.Button("Dashboard", _settingsEditor.buttonBlue)) 
                Application.OpenURL("https://apps.admob.com/v2/home");
            if (GUILayout.Button("Get plugin", _settingsEditor.buttonBlue)) 
                Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases/latest");
            if (GUILayout.Button("Start guide (AdMob Documentation)", _settingsEditor.buttonBlue)) 
                Application.OpenURL("https://developers.google.com/admob/unity/start");
        }

        private void DrawAdsUnityAds()
        {
            GUILayout.Label("Unity Ads", _settingsEditor.labelTitle);

            //Values
            _unityAdsOn.boolValue = EditorGUILayout.Toggle("On: ", _unityAdsOn.boolValue);
            EditorGUILayout.Space(8);

            GUILayout.Label("AppId:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _unityAdsAppIdIOS.stringValue = EditorGUILayout.TextArea(_unityAdsAppIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _unityAdsAppIdAndroid.stringValue = EditorGUILayout.TextArea(_unityAdsAppIdAndroid.stringValue);

            EditorGUILayout.Space(12);
            GUILayout.Label("Banner:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _unityAdsBannerIdIOS.stringValue = EditorGUILayout.TextArea(_unityAdsBannerIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _unityAdsBannerIdAndroid.stringValue = EditorGUILayout.TextArea(_unityAdsBannerIdAndroid.stringValue);

            EditorGUILayout.Space(4);
            GUILayout.Label("Interstitial:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _unityAdsInterstitialIdIOS.stringValue = EditorGUILayout.TextArea(_unityAdsInterstitialIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _unityAdsInterstitialIdAndroid.stringValue = EditorGUILayout.TextArea(_unityAdsInterstitialIdAndroid.stringValue);

            EditorGUILayout.Space(4);
            GUILayout.Label("Interstitial:", _settingsEditor.labelParameter);
            GUILayout.Label("iOS", _settingsEditor.labelDescription);
            _unityAdsRewardedVideoIdIOS.stringValue = EditorGUILayout.TextArea(_unityAdsRewardedVideoIdIOS.stringValue);
            GUILayout.Label("Android", _settingsEditor.labelDescription);
            _unityAdsRewardedVideoIdAndroid.stringValue = EditorGUILayout.TextArea(_unityAdsRewardedVideoIdAndroid.stringValue);
            EditorGUILayout.Space(10);
            GUILayout.Label("Interaction", _settingsEditor.labelParameter);
            EditorGUILayout.BeginHorizontal();
            /*
            if (GUILayout.Button("Integrate", _settingsEditor.buttonYellow))
                UnityAdIntegrator.Integrate();
            if (GUILayout.Button("Disintegrate", _settingsEditor.buttonRed)) 
                UnityAdIntegrator.Disintegrate();*/
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            
            GUILayout.Label("Helps", _settingsEditor.labelParameter);
            if (GUILayout.Button("Dashboard", _settingsEditor.buttonBlue)) 
                Application.OpenURL("https://operate.dashboard.unity3d.com");
            if (GUILayout.Button("Get plugin", _settingsEditor.buttonBlue)) 
                Application.OpenURL("https://developers.google.com/admob/unity/quick-start");
            if (GUILayout.Button("Start guide (Unity Ads Documentation)", _settingsEditor.buttonBlue)) 
                Application.OpenURL("https://unityads.unity3d.com/help/monetization/getting-started");
        }

        private void DrawAdsShowOptions()
        {
            GUILayout.Label("Show Options", _settingsEditor.labelTitle);
            if (GUILayout.Button("Options", _settingsEditor.buttonYellow)) 
                _isAdsShownOptionsFolded = !_isAdsShownOptionsFolded;
            if (_isAdsShownOptionsFolded)
            {
                for (int i = 0; i < _shownOptions.arraySize; i++)
                {
                    SerializedProperty dataElement = _shownOptions.GetArrayElementAtIndex(i);
                    SerializedProperty scene = dataElement.FindPropertyRelative("scene");
                    SerializedProperty bannerShowAtStart = dataElement.FindPropertyRelative("bannerShowAtStart");
                    SerializedProperty interstitialShowAtStart = dataElement.FindPropertyRelative("interstitialShowAtStart");

                    if (i % 2 == 0)
                        Separator(60);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.Space(4);
                    EditorGUILayout.PropertyField(scene, new GUIContent("Scene:"));
                    EditorGUILayout.EndVertical();
                    if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                    {
                        _shownOptions.DeleteArrayElementAtIndex(i);
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Show at start of scene");
                    EditorGUILayout.Separator();
                    EditorGUILayout.PropertyField(bannerShowAtStart, new GUIContent("Banner"));
                    EditorGUILayout.PropertyField(interstitialShowAtStart, new GUIContent("Interstitial"));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
                if (GUILayout.Button("Add option", _settingsEditor.buttonGreen)) 
                    _shownOptions.arraySize++;
            }

            GUILayout.Label("Other Scenarios", _settingsEditor.labelParameter);
            _showRewardVideoWhenLose.boolValue = EditorGUILayout.Toggle("Show reward when lose: ", _showRewardVideoWhenLose.boolValue);
            EditorGUILayout.Space(4);
            _showDoubleCoinsAtDailyReward.boolValue = EditorGUILayout.Toggle("Double coins at daily reward: ", _showDoubleCoinsAtDailyReward.boolValue);
            _showDoubleCoinsAtDailyTask.boolValue = EditorGUILayout.Toggle("Double coins at daily task: ", _showDoubleCoinsAtDailyTask.boolValue);
            EditorGUILayout.Space(4);

            GUILayout.Label("Debug", _settingsEditor.labelParameter);
            _debugMessagesOn.boolValue = EditorGUILayout.Toggle("Debug log messages: ", _debugMessagesOn.boolValue);
            EditorGUILayout.Space(4);

            AssetDatabase.SaveAssets();
        }

        #endregion

        private void DrawIAP()
        {
            GUILayout.Label("In-Apps", _settingsEditor.labelTitle);

            _inAppsOn.boolValue = EditorGUILayout.Toggle("On: ", _inAppsOn.boolValue);

            GUILayout.Label("Products", _settingsEditor.labelParameter);
            if (GUILayout.Button("Basic Products", _settingsEditor.buttonYellow)) 
                _isInAppBasicFolded = !_isInAppBasicFolded;
            if (_isInAppBasicFolded)
            {
                PrintInAppArray(_inAppsBasic);
                EditorGUILayout.Space(15);
            }
            if (GUILayout.Button("Coins Products", _settingsEditor.buttonYellow)) 
                _isInAppCoinsFolded = !_isInAppCoinsFolded;
            if (_isInAppCoinsFolded)
            {
                PrintInAppArray(_inAppCoins);
            }

            EditorGUILayout.Space(4);
            GUILayout.Label("Debug", _settingsEditor.labelParameter);
            _inAppsDebugMessagesOn.boolValue = EditorGUILayout.Toggle("Debug log messages: ", _inAppsDebugMessagesOn.boolValue);
            EditorGUILayout.Space(4);
        }

        private void PrintInAppArray(SerializedProperty array)
        {
            if (array == null)
            {
                Debug.LogError("array is null...");
                return;
            }
            
            for (int i = 0; i < array.arraySize; i++)
            {
                    SerializedProperty dataElement = array.GetArrayElementAtIndex(i);
                    SerializedProperty idProperty = dataElement.FindPropertyRelative("id");
                    SerializedProperty onProperty = dataElement.FindPropertyRelative("on");
                    SerializedProperty typeProperty = dataElement.FindPropertyRelative("type");
                    SerializedProperty titleProperty = dataElement.FindPropertyRelative("title");
                    SerializedProperty valueProperty = dataElement.FindPropertyRelative("value");
                    SerializedProperty iconProperty = dataElement.FindPropertyRelative("icon");
                    SerializedProperty bestDealIndicatorProperty = dataElement.FindPropertyRelative("bestDealIndicator");

                    if (i % 2 == 0)
                        Separator(typeProperty.enumValueIndex != 0 ? 175 : 155);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(titleProperty.stringValue + " - " + idProperty.stringValue, EditorStyles.boldLabel);
                    if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                    {
                        array.DeleteArrayElementAtIndex(i);
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(idProperty);
                    EditorGUILayout.PropertyField(onProperty);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(typeProperty);
                    EditorGUILayout.PropertyField(titleProperty);
                    if (typeProperty.enumValueIndex != 0)
                        EditorGUILayout.PropertyField(valueProperty);
                    EditorGUILayout.PropertyField(iconProperty);
                    EditorGUILayout.PropertyField(bestDealIndicatorProperty);
                    EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add new In-App", _settingsEditor.buttonGreen)) 
                array.arraySize++;
        }

        private void DrawNotification()
        {
            GUILayout.Label("Notifications", _settingsEditor.labelTitle);

            _notificationOn.boolValue = EditorGUILayout.Toggle("On: ", _notificationOn.boolValue);
            EditorGUILayout.Space(15);

            //Data
            GUILayout.Label("Data", _settingsEditor.labelTitle);
            if (_soSettingsCommon != null && _notificationDataProperty != null)
            {
                for (int i = 0; i < _notificationDataProperty.arraySize; i++)
                {
                    //Values
                    SerializedProperty dataElement = _notificationDataProperty.GetArrayElementAtIndex(i);
                    SerializedProperty titleProperty = dataElement.FindPropertyRelative("title");
                    SerializedProperty delayHoursProperty = dataElement.FindPropertyRelative("delayHours");
                    SerializedProperty textProperty = dataElement.FindPropertyRelative("text");

                    Separator(80);

                    //Add name of element and remove button
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(titleProperty.stringValue, EditorStyles.boldLabel);
                    if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                    {
                        _notificationDataProperty.DeleteArrayElementAtIndex(i);
                        EditorGUILayout.EndHorizontal();
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    //Value of notifications
                    EditorGUILayout.PropertyField(titleProperty);
                    EditorGUILayout.PropertyField(delayHoursProperty);
                    EditorGUILayout.Space();

                    //Draw list of possible notification text
                    DrawNestedArrayProperty(_notificationDataProperty, i, textProperty);
                }

                EditorGUILayout.Space();

                //Button to add a new element
                if (GUILayout.Button("Add Notification Data", _settingsEditor.buttonGreen)) 
                    _notificationDataProperty.arraySize++;
            }
            else
            {
                EditorGUILayout.HelpBox("Settings Common or Notification Data is null.", MessageType.Warning);
            }
        }

        public void DrawAudio()
        {
            GUILayout.Label("Audio", _settingsEditor.labelTitle);

            EditorGUILayout.Space(10);
            GUILayout.Label("Interface", _settingsEditor.labelParameter);
            EditorGUILayout.PropertyField(_audioButtonClick, new GUIContent("Button Click:"));
            EditorGUILayout.PropertyField(_audioSwitch, new GUIContent("Switch:"));
            EditorGUILayout.PropertyField(_audioError, new GUIContent("Error:"));
            EditorGUILayout.PropertyField(_audioSuccessful, new GUIContent("Successful:"));
            EditorGUILayout.PropertyField(_audioEquip, new GUIContent("Equip:"));

            EditorGUILayout.Space(5);
            GUILayout.Label("Daily", _settingsEditor.labelParameter);
            EditorGUILayout.PropertyField(_audioDailyRewardWheel, new GUIContent("Daily reward wheel:"));
            EditorGUILayout.PropertyField(_audioDailyTaskNotification, new GUIContent("Daily task notification:"));

            EditorGUILayout.Space(5);
            GUILayout.Label("Game", _settingsEditor.labelParameter);
            EditorGUILayout.PropertyField(_audioCoin, new GUIContent("Get coin:"));
            EditorGUILayout.PropertyField(_audioPoint, new GUIContent("Get point:"));
            EditorGUILayout.PropertyField(_audioBrake, new GUIContent("Brake:"));
            EditorGUILayout.PropertyField(_audioJump, new GUIContent("Jump:"));
            EditorGUILayout.PropertyField(_audioTurn, new GUIContent("Turn:"));
            EditorGUILayout.PropertyField(_audioSaw, new GUIContent("Saw:"));
            EditorGUILayout.PropertyField(_audioEngine, new GUIContent("Engine:"));
            EditorGUILayout.PropertyField(_audioSpeedUp, new GUIContent("Speed up:"));
            EditorGUILayout.PropertyField(_audioSlowDown, new GUIContent("Slow down:"));

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_audioSmash1, new GUIContent("Smash 1:"));
            EditorGUILayout.PropertyField(_audioSmash2, new GUIContent("Smash 2:"));

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_audioStartLinePrepare, new GUIContent("Start line prepare:"));
            EditorGUILayout.PropertyField(_audioStartLineRun, new GUIContent("Start line run:"));
            EditorGUILayout.PropertyField(_audioLose, new GUIContent("Lose:"));
            EditorGUILayout.PropertyField(_audioWin, new GUIContent("Win:"));

            EditorGUILayout.Space(10);
            GUILayout.Label("Music", _settingsEditor.labelParameter);
            EditorGUILayout.PropertyField(_musicGame, new GUIContent("Game:"));
            EditorGUILayout.PropertyField(_musicMenu, new GUIContent("Menu:"));
        }

        private void Separator(int height)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = height;
            EditorGUI.DrawRect(rect, _settingsEditor.backgroundFrameColor);
        }

        #endregion

        #region Components

        private void DrawNestedArrayProperty(SerializedProperty parentProperty, int index, SerializedProperty arrayProperty)
        {
            //Add labels and add/remove buttons
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Texts", EditorStyles.boldLabel);
            if (GUILayout.Button("+", _settingsEditor.buttonGreen, GUILayout.Width(26))) 
                arrayProperty.arraySize++;
            if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26))) 
                arrayProperty.arraySize--;
            EditorGUILayout.EndHorizontal();

            //Draw fields
            for (int i = 0; i < arrayProperty.arraySize; i++)
                EditorGUILayout.PropertyField(arrayProperty.GetArrayElementAtIndex(i), true);

            EditorGUI.indentLevel--;
        }

        #endregion
    }
}
#endif
