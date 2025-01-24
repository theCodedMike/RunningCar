#if UNITY_EDITOR
using Editor.Components;
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WelcomeMenuEditor : EditorWindow
    {
        private Vector2 _scrollPosition;
        private static SettingsEditor _settingsEditor;
        private const string SettingsEditorAddress = "Assets/ScriptableObjects/Settings/SettingsEditor.asset";

        
        
        [MenuItem("Tools/RunningCar/Welcome Menu", false, 0)]
        public static void ShowWindow()
        {
            GetWindow<WelcomeMenuEditor>("RunningCar - Welcome").minSize = new Vector2(650, 250);

            EditorApplication.delayCall -= ShowWindow;
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(SettingsEditorAddress);

            if (EditorApplication.timeSinceStartup < 20)
                EditorApplication.delayCall += ShowWindow;
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.Space(10);

            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, CustomGUIStyle.Box(_settingsEditor.backgroundColor));

            DrawHead();
            DrawTools();
            DrawMoreProjects();

            EditorGUILayout.EndScrollView();
        }

        private void DrawHead()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.Box(new Rect(0, 0, position.width, 120), GUIContent.none, CustomGUIStyle.Box(_settingsEditor.backgroundFrameColor));
            EditorGUILayout.Space(5);
            GUILayout.Label(_settingsEditor.logo, GUILayout.MaxWidth(120), GUILayout.MaxHeight(100));
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Hello! I'm Alice from PopPopGames.", _settingsEditor.labelTitleLarge);
            EditorGUILayout.Space(5);
            GUILayout.Label("Thanks for purchasing my game template!", _settingsEditor.labelParameterNarrow);
            GUILayout.Label("If you have some questions read documentation.", _settingsEditor.labelParameterNarrow);
            GUILayout.Label("Please, leave a feedback and rate project at asset store!", _settingsEditor.labelParameterNarrow);
            EditorGUILayout.Space(8);
            GUILayout.Label("Project version: " + _settingsEditor.versionNumber, _settingsEditor.labelParameterNarrow);
            EditorGUILayout.EndVertical();
            GUILayout.ExpandWidth(true);
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Documentation", _settingsEditor.buttonYellow, GUILayout.Width(160)))
                Application.OpenURL(_settingsEditor.linkDocumentation);
            EditorGUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("About Us", _settingsEditor.buttonGray, GUILayout.Width(79)))
                Application.OpenURL(_settingsEditor.linkAboutMe);
            if (GUILayout.Button("Support", _settingsEditor.buttonBlue, GUILayout.Width(79)))
                Application.OpenURL(_settingsEditor.linkSupport);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
            if (GUILayout.Button("Rate Project", _settingsEditor.buttonGreen, GUILayout.Width(160)))
                Application.OpenURL(_settingsEditor.linkRateProject);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTools()
        {
            EditorGUILayout.Space(10);
            GUILayout.Label("TOOLS", _settingsEditor.labelTitle);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(CustomGUIContent.GetImage(_settingsEditor.iconSkin, "  Skin Editor"), _settingsEditor.buttonBlack))
                GetWindow<SkinHolderEditor>("RunningCar - Skins");
            if (GUILayout.Button(CustomGUIContent.GetImage(_settingsEditor.iconLevel, "  Level Editor"), _settingsEditor.buttonBlack))
                GetWindow<LevelEditor>("RunningCar - Level Editor");
            if (GUILayout.Button(CustomGUIContent.GetImage(_settingsEditor.iconSettings, "  Settings Common"), _settingsEditor.buttonBlack))
                GetWindow<SettingsCommonEditor>("RunningCar - Settings Common");
            if (GUILayout.Button(CustomGUIContent.GetImage(_settingsEditor.iconSettings, "  Settings Game"), _settingsEditor.buttonBlack))
                GetWindow<SettingsGameEditor>("RunningCar - Settings Game");
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMoreProjects()
        {
            if (_settingsEditor.otherProjects.Length > 0)
            {
                EditorGUILayout.Space(10);
                GUILayout.Label("MORE PROJECTS", _settingsEditor.labelTitle);

                var counter = 0;
                var maxButtonsInLine = 4;
                var isBeginActive = false;
                for (int i = 0; i < _settingsEditor.otherProjects.Length; i++)
                {
                    if (counter == 0)
                    {
                        isBeginActive = true;
                        EditorGUILayout.BeginHorizontal();
                    }

                    var otherProject = _settingsEditor.otherProjects[i];
                    var style = _settingsEditor.buttonBlack;
                    style.alignment = TextAnchor.LowerCenter;
                    style.imagePosition = ImagePosition.ImageAbove;
                    var layout = new[] { GUILayout.Width(200), GUILayout.Height(152) };
                    if (GUILayout.Button(CustomGUIContent.GetImage(otherProject.icon, " " + otherProject.name), style, layout))
                        Application.OpenURL(otherProject.link);

                    counter++;
                    if (counter == maxButtonsInLine)
                    {
                        counter = 0;
                        isBeginActive = false;
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (isBeginActive)
                    EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif