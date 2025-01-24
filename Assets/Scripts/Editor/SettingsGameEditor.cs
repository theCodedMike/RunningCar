#if UNITY_EDITOR
using Editor.Components;
using Interface.DailyReward;
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SettingsGameEditor : EditorWindow
    {
        // 组件
        private int _toolbarInt;
        private readonly string[] _toolbarStrings = { "Player", "Daily Reward", "Daily Task", "Options" };
        private bool _isShowDailyRewardsData = true;
        
        private Vector2 _scrollPosition;
        private static SettingsEditor _settingsEditor;
        private const string SettingsEditorAddress = "Assets/ScriptableObjects/Settings/SettingsEditor.asset";
        private static SerializedObject _soSettingsGame;

        
        // Values
        private static SerializedProperty _speed;
        private static SerializedProperty _steering;
        private static SerializedProperty _velocity;
        private static SerializedProperty _jumpDistance;
        private static SerializedProperty _jumpHeight;
        private static SerializedProperty _decreaseSpeedAtTurnTo;
        
        private static SerializedProperty _dailyRewardIsOn;
        private static SerializedProperty _dailyRewardDatas;
        
        private static SerializedProperty _dailyTaskIsOn;
        private static SerializedProperty _dailyTasks;
        private static SerializedProperty _dailyTaskRewards;
        
        
        
        [MenuItem("Tools/RunningCar/Settings Game", false, 41)]
        public static void ShowWindow()
        {
            GetWindow<SettingsGameEditor>("RunningCar - Settings Game");
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(SettingsEditorAddress);

            _soSettingsGame = new SerializedObject(_settingsEditor.settingsGame);
            _soSettingsGame.Update();

            _speed = _soSettingsGame.FindProperty("speed");
            _steering = _soSettingsGame.FindProperty("steering");
            _velocity = _soSettingsGame.FindProperty("velocity");
            _jumpDistance = _soSettingsGame.FindProperty("jumpDistance");
            _jumpHeight = _soSettingsGame.FindProperty("jumpHeight");
            _decreaseSpeedAtTurnTo = _soSettingsGame.FindProperty("decreaseSpeedAtTurnTo");

            _dailyRewardIsOn = _soSettingsGame.FindProperty("dailyRewardIsOn");
            _dailyRewardDatas = _soSettingsGame.FindProperty("dailyRewardDatas");

            _dailyTaskIsOn = _soSettingsGame.FindProperty("dailyTaskIsOn");
            _dailyTasks = _soSettingsGame.FindProperty("dailyTasks");
            _dailyTaskRewards = _soSettingsGame.FindProperty("dailyTaskRewards");
        }

        private void OnGUI()
        {
            _soSettingsGame.Update();

            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, CustomGUIStyle.Box(_settingsEditor.backgroundColor));

            _toolbarInt = GUILayout.Toolbar(_toolbarInt, _toolbarStrings, _settingsEditor.toolbarMain, CustomGUILayout.ToolBar());
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.Space(10);
            switch (_toolbarInt)
            {
                case 0:
                    DrawPlayer();
                    break;
                case 1:
                    DrawDailyRewards();
                    break;
                case 2:
                    DrawDailyTask();
                    break;
                default:
                    DrawOptions();
                    break;
            }
            EditorGUILayout.EndScrollView();

            _soSettingsGame.ApplyModifiedProperties();
        }

        private void DrawPlayer()
        {
            GUILayout.Label("Player", _settingsEditor.labelTitle);

            EditorGUILayout.PropertyField(_speed, new GUIContent("Speed: "));
            EditorGUILayout.PropertyField(_steering, new GUIContent("Steering: "));
            EditorGUILayout.PropertyField(_velocity, new GUIContent("Velocity: "));

            GUILayout.Label("Jumps", _settingsEditor.labelTitle);
            EditorGUILayout.PropertyField(_jumpDistance, new GUIContent("Distance: "));
            EditorGUILayout.PropertyField(_jumpHeight, new GUIContent("Height: "));

            GUILayout.Label("Turns", _settingsEditor.labelTitle);
            EditorGUILayout.PropertyField(_decreaseSpeedAtTurnTo, new GUIContent("Decrease speed: "));
        }

        void DrawDailyRewards()
        {
            GUILayout.Label("Daily Rewards", _settingsEditor.labelTitle);
            EditorGUILayout.PropertyField(_dailyRewardIsOn, new GUIContent("On: "));

            GUILayout.Label("Data", _settingsEditor.labelTitle);
            if (GUILayout.Button("Segments", _settingsEditor.buttonYellow)) 
                _isShowDailyRewardsData = !_isShowDailyRewardsData;
            if (_isShowDailyRewardsData)
            {
                if (_dailyRewardDatas != null)
                {
                    for (int i = 0; i < _dailyRewardDatas.arraySize; i++)
                    {
                        var data = _dailyRewardDatas.GetArrayElementAtIndex(i);
                        var type = data.FindPropertyRelative("type");
                        var value = data.FindPropertyRelative("value");

                        if (i % 2 == 0)
                            Separator(21);

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Segment " + (i + 1).ToString());
                        EditorGUILayout.PropertyField(type);
                        if (_settingsEditor.settingsGame.dailyRewardDatas[i].type != DailyRewardType.Nothing)
                            EditorGUILayout.PropertyField(value);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }

        void DrawDailyTask()
        {
            GUILayout.Label("Daily Task", _settingsEditor.labelTitle);

            EditorGUILayout.PropertyField(_dailyTaskIsOn, new GUIContent("On: "));

            EditorGUILayout.Space(5);
            GUILayout.Label("Rewards", _settingsEditor.labelParameter);
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < _dailyTaskRewards.arraySize; i++)
            {
                EditorGUILayout.BeginVertical();

                var element = _dailyTaskRewards.GetArrayElementAtIndex(i);

                GUILayout.Label("Day " + (i + 1).ToString(), _settingsEditor.labelDescription);
                EditorGUILayout.PropertyField(element, new GUIContent(""));

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            GUILayout.Label("Tasks", _settingsEditor.labelParameter);
            for (int i = 0; i < _dailyTasks.arraySize; i++)
            {
                var dataElement = _dailyTasks.GetArrayElementAtIndex(i);
                var typeProperty = dataElement.FindPropertyRelative("type");
                var valuesProperty = dataElement.FindPropertyRelative("values");

                if (i % 2 == 0)
                    Separator(30);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(typeProperty);
                EditorGUILayout.PropertyField(valuesProperty);
                if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                {
                    _dailyTasks.DeleteArrayElementAtIndex(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(2);
            }
            if (GUILayout.Button("Add Task", _settingsEditor.buttonGreen)) 
                _dailyTasks.arraySize++;
        }

        void DrawOptions()
        {
            GUILayout.Label("Options", _settingsEditor.labelTitle);

            EditorGUILayout.Space(10);
            //GUILayout.Label("Restore", settingsEditor.labelTitle);
            if (GUILayout.Button("Restore to default", _settingsEditor.buttonRed))
            {
                if (EditorUtility.DisplayDialog("Restore to default", "This action remove all stats. Are you sure?", "Yes", "Cancel"))
                {
                    PlayerPrefs.DeleteAll();
                    Debug.Log("Restore to default: Done");
                }
            }
            EditorGUILayout.HelpBox("Restore all settings and progress to default state. (Delete all PlayerPrefs)", MessageType.Info);
        }

        private void Separator(int height)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = height;
            EditorGUI.DrawRect(rect, _settingsEditor.backgroundFrameColor);
        }
    }
}
#endif