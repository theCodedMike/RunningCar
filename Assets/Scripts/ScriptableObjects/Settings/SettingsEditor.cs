using System;
using UnityEngine;

namespace ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsEditor", menuName = "RunningCar/Settings/Editor", order = 25)]
    public class SettingsEditor : ScriptableObject
    {
        [Header("资源")]
        public SettingsCommon settingsCommon;
        public SettingsAds settingsAds;
        public SettingsInApps settingsInApps;
        public SettingsAudio settingsAudio;
        public SettingsGame settingsGame;
        
        [Header("背景")]
        public Color backgroundColor = Color.black;
        public Color backgroundFrameColor = Color.gray;

        [Header("按钮")]
        public GUIStyle buttonRed;
        public GUIStyle buttonBlue;
        public GUIStyle buttonGreen;
        public GUIStyle buttonYellow;
        public GUIStyle buttonGray;
        public GUIStyle buttonBlack;
        
        [Header("ToolBar")]
        public GUIStyle toolbarMain;
        public GUIStyle toolbarSmall;
        
        [Header("Label")]
        public GUIStyle labelTitle;
        public GUIStyle labelTitleLarge;
        public GUIStyle labelDescription;
        public GUIStyle labelParameter;
        public GUIStyle labelParameterNarrow;

        [Header("欢迎按钮")]
        public Texture2D logo;

        [Space()]
        public string linkDocumentation = "";
        public string linkAboutMe = "";
        public string linkSupport = "";
        public string linkRateProject = "";
        public string versionNumber = "1.0";

        [Space()]
        public Texture2D iconSkin;
        public Texture2D iconLevel;
        public Texture2D iconSettings;

        [Space()]
        public SettingsEditorOtherProjectLinks[] otherProjects;

        [Header("关卡编辑器")]
        public bool levelEditorSetNewObjectToPreviousPosition = true;
        public bool levelEditorSetNewObjectToPreviousRotation = true;
    }
    
    
    [Serializable]
    public class SettingsEditorOtherProjectLinks
    {
        public Texture2D icon;
        public string name;
        public string link;
    }
}
