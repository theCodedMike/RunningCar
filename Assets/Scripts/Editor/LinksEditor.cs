#if UNITY_EDITOR
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LinksEditor : EditorWindow
    {
        private static SettingsEditor _settingsEditor;
        private const string SettingsEditorAddress = "Assets/ScriptableObjects/Settings/SettingsEditor.asset";

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(SettingsEditorAddress);
        }

        [MenuItem("Tools/RunningCar/Documentation", false, 1000)]
        public static void OpenDocumentation()
        {
            Application.OpenURL(_settingsEditor.linkDocumentation);
        }

        [MenuItem("Tools/RunningCar/About Us", false, 1000)]
        public static void OpenAboutUs()
        {
            Application.OpenURL(_settingsEditor.linkAboutMe);
        }
        
        [MenuItem("Tools/RunningCar/Support", false, 1000)]
        public static void OpenSupport()
        {
            Application.OpenURL(_settingsEditor.linkSupport);
        }
        
        [MenuItem("Tools/RunningCar/Rate Project", false, 1000)]
        public static void OpenRateProject()
        {
            Application.OpenURL(_settingsEditor.linkRateProject);
        }
    }
}
#endif