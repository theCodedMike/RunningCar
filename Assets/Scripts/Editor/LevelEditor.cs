#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Utility;
using Editor.Components;
using Interface.LevelEditor;
using ScriptableObjects.GamePrefabs;
using ScriptableObjects.Level;
using ScriptableObjects.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    public class LevelEditor : EditorWindow
    {
        private static LevelHolder _levelHolder;
        private static LevelHolder _levelHolderLoaded;
        private static GamePrefabHolder _gamePrefabHolder;
        private static GamePrefabHolder _gamePrefabHolderLoaded;
        private static SettingsEditor _settingsEditor;
        private const string AssetAddressSettingsEditor = "Assets/ScriptableObjects/Settings/SettingsEditor.asset";
        private const string AssetAddressLevelHolder = "Assets/ScriptableObjects/Levels/LevelHolder.asset";
        private const string AssetAddressGamePrefabHolder = "Assets/ScriptableObjects/Levels/GamePrefabHolder.asset";
        private const string SettingsEditorAddress = AssetAddressSettingsEditor;

        public enum Type
        {
            Main, Specific, Modify,
            Options,
            LevelObjects, SelectedLevelObject
        }

        private Type _currentType = Type.Main;
        private Vector2 _scrollPositionMain;
        private Vector2 _scrollPositionMainWarnings;
        private Vector2 _scrollPositionModifyObjects;
        private Vector2 _scrollPositionModifyPrefabsSelector;

        private int _toolbarLevelObjectsTypeSelected = 0;
        private readonly string[] _toolbarLevelObjectsType = { "Basic", "Grounds", "Other" };

        private static SerializedProperty _levelHolderDataProperty;
        private static SerializedObject _soLevelHolder;
        private static SerializedObject _soEditor;

        private static SerializedProperty _selectedLevel;
        private static SerializedProperty _selectedLevelObject;
        
        private static SerializedProperty _gamePrefabDataBasic;
        private static SerializedProperty _gamePrefabDataGrounds;
        private static SerializedProperty _gamePrefabDataOther;
        private static SerializedObject _soGamePrefabHolder;
        
        private static SerializedProperty _levelEditorSetNewObjectToPreviousPosition;
        private static SerializedProperty _levelEditorSetNewObjectToPreviousRotation;

        private static int _selectedLevelId;
        private static int _selectedLevelObjectId;
        private static int _popupLevelObjectId = -1;

        
        [MenuItem("Tools/RunningCar/LevelEditor", false, 21)]
        public static void ShowWindow()
        {
            GetWindow<LevelEditor>("RunningCar - Level Editor");
        }

        public void OnInspectorUpdate()
        {
            if (_currentType == Type.Modify)
            {
                _soLevelHolder.Update();
                Repaint();
            }
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            _levelHolder = AssetDatabase.LoadAssetAtPath<LevelHolder>(AssetAddressLevelHolder);
            _gamePrefabHolder = AssetDatabase.LoadAssetAtPath<GamePrefabHolder>(AssetAddressGamePrefabHolder);
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(SettingsEditorAddress);

            _soEditor = new SerializedObject(_settingsEditor);

            if (_levelHolder)
            {
                _levelHolderLoaded = _levelHolder;

                _soLevelHolder = new SerializedObject(_levelHolder);
                _soLevelHolder.Update();

                _levelHolderDataProperty = _soLevelHolder.FindProperty("levels");
            }

            if (_gamePrefabHolder)
            {
                _gamePrefabHolderLoaded = _gamePrefabHolder;
                
                _soGamePrefabHolder = new SerializedObject(_gamePrefabHolder);
                _soGamePrefabHolder.Update();

                _gamePrefabDataBasic = _soGamePrefabHolder.FindProperty("basic");
                _gamePrefabDataGrounds = _soGamePrefabHolder.FindProperty("grounds");
                _gamePrefabDataOther = _soGamePrefabHolder.FindProperty("other");
            }

            _levelEditorSetNewObjectToPreviousPosition = _soEditor.FindProperty("levelEditorSetNewObjectToPreviousPosition");
            _levelEditorSetNewObjectToPreviousRotation = _soEditor.FindProperty("levelEditorSetNewObjectToPreviousRotation");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            
            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, CustomGUIStyle.Box(_settingsEditor.backgroundColor));

            switch (_currentType)
            {
                case Type.Main: DrawMain(); break;
                case Type.Specific: DrawSpecific(); break;
                case Type.Modify: DrawModify(); break;
                case Type.Options: DrawOptions(); break;
                case Type.LevelObjects: DrawLevelObjects(); break;
                case Type.SelectedLevelObject: DrawSelectedLevelObjects(); break;
                default: Debug.LogError($"Unknown type: {_currentType}"); break;
            }

            _soLevelHolder.ApplyModifiedProperties();
        }

        private void DrawMain()
        {
            GUILayout.Label("Level list", _settingsEditor.labelTitle);
            EditorGUILayout.Space(10);

            _selectedLevel = null;
            _selectedLevelId = 0;
            _popupLevelObjectId = -1;

            //Warning about no stars
            var starStatus = _levelHolder.GetLevelsStarStatus();
            if (starStatus.Count > 0)
            {
                _scrollPositionMainWarnings = EditorGUILayout.BeginScrollView(_scrollPositionMainWarnings, GUILayout.Height(50));
                foreach (var status in starStatus)
                    EditorGUILayout.HelpBox(status, MessageType.Warning);
                EditorGUILayout.EndScrollView();
            }

            //Draw a list of levels
            if (_levelHolder != null && _soLevelHolder != null && _levelHolderDataProperty != null)
            {
                _soLevelHolder.Update();
                _scrollPositionMain = EditorGUILayout.BeginScrollView(_scrollPositionMain);
                for (int i = 0; i < _levelHolderDataProperty.arraySize; i++)
                {
                    SerializedProperty dataElement = _levelHolderDataProperty.GetArrayElementAtIndex(i);
                    SerializedProperty name = dataElement.FindPropertyRelative("name");

                    if (i % 2 == 0) Separator(31);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Level {i + 1} - {name.stringValue}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Edit", _settingsEditor.buttonYellow, GUILayout.Width(70)))
                    {
                        _selectedLevel = dataElement;
                        _selectedLevelId = i;
                        ChangeTypeTo(Type.Specific);
                    }
                    GUILayout.Space(15);
                    if (_levelHolderDataProperty.arraySize > 1)
                    {
                        if (i != 0)
                        {
                            if (GUILayout.Button("↑", _settingsEditor.buttonGray, GUILayout.Width(26))) _levelHolderDataProperty.MoveArrayElement(i, i - 1);
                        }
                        else
                            GUILayout.Space(29);
                        if (i != _levelHolderDataProperty.arraySize - 1)
                        {
                            if (GUILayout.Button("↓", _settingsEditor.buttonGray, GUILayout.Width(26))) _levelHolderDataProperty.MoveArrayElement(i, i + 1);
                        }
                        else
                            GUILayout.Space(29);
                    }
                    GUILayout.Space(15);
                    if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                    {
                        if (EditorUtility.DisplayDialog("Delete", "Are you sure that you want to delete this level?", "Yes", "Cancel"))
                            _levelHolderDataProperty.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(2);
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.Space(5);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create New", _settingsEditor.buttonGreen, CustomGUILayout.ButtonLarge())) 
                CreateNewLevel();
            if (GUILayout.Button("Level Objects", _settingsEditor.buttonBlue, CustomGUILayout.ButtonLarge(110))) 
                _currentType = Type.LevelObjects;
            if (GUILayout.Button("Options", _settingsEditor.buttonGray, CustomGUILayout.ButtonLarge(80))) 
                _currentType = Type.Options;
            EditorGUILayout.EndHorizontal();

            _soLevelHolder.ApplyModifiedProperties();
        }
        
        private void DrawSpecific()
        {
            if (_selectedLevel == null)
            {
                ChangeTypeTo(Type.Main);
                return;
            }

            _selectedLevel.serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _settingsEditor.buttonGray, GUILayout.Width(80)))
            {
                GUI.FocusControl(null);
                EditorSceneManager.OpenScene("Assets/Scenes/MenuScene.scene");
                ChangeTypeTo(Type.Main);
            }
            GUILayout.Label($"Level {_selectedLevelId + 1}", _settingsEditor.labelTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(15);

            var starStatus = _levelHolder.GetStarStatusForLevelId(_selectedLevelId);
            if (starStatus != null)
                EditorGUILayout.HelpBox(starStatus, MessageType.Warning);

            var name = _selectedLevel.FindPropertyRelative("name");
            var sawSpeed = _selectedLevel.FindPropertyRelative("sawSpeed");

            EditorGUILayout.PropertyField(name);
            EditorGUILayout.Space(5);
            GUILayout.Label($"Values", _settingsEditor.labelParameter);
            EditorGUILayout.PropertyField(sawSpeed);
            EditorGUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", _settingsEditor.buttonYellow))
            {
                if (EditorUtility.DisplayDialog("Clear", "Are you sure that you want to remove all items from this level?", "Yes", "Cancel"))
                {
                    ClearLevel(_selectedLevelId);

                    if (LevelEditorPreview.Instance != null)
                        LevelEditorPreview.Instance.DrawObjects();
                }
            }
            if (GUILayout.Button("Delete", _settingsEditor.buttonRed, GUILayout.Width(100)))
            {
                if (EditorUtility.DisplayDialog("Delete", "Are you sure that you want to delete this level?", "Yes", "Cancel"))
                {
                    GUI.FocusControl(null);
                    _levelHolderDataProperty.DeleteArrayElementAtIndex(_selectedLevelId);
                    ChangeTypeTo(Type.Main);

                    Debug.Log("Level editor: Level deleted");
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            DrawModify();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            _selectedLevel.serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawModify()
        {
            if (_selectedLevel == null)
            {
                ChangeTypeTo(Type.Main);
                return;
            }
            _soGamePrefabHolder.Update();

            //var level = _levelHolder.levels[_selectedLevelId];
            List<LevelObject> levelObjects = _levelHolder.levels[_selectedLevelId].objects;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Editor tools", _settingsEditor.labelTitle);
            if (GUILayout.Button("Save", _settingsEditor.buttonGreen, GUILayout.Width(80))) 
                Save();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            SerializedProperty objects = _selectedLevel.FindPropertyRelative("objects");

            _scrollPositionModifyObjects = EditorGUILayout.BeginScrollView(_scrollPositionModifyObjects);
            for (int i = 0; i < objects.arraySize; i++)
            {
                if (i % 2 == 0) 
                    Separator(31);

                LevelObject obj = levelObjects[i];
                SerializedProperty objSerialised = objects.GetArrayElementAtIndex(i);
                bool isSameAsPopUpped = i == _popupLevelObjectId;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(obj.prefab.name);
                if (GUILayout.Button(isSameAsPopUpped ? "Hide stats" : "Show stats", isSameAsPopUpped ? _settingsEditor.buttonRed : _settingsEditor.buttonBlack, GUILayout.Width(90)))
                {
                    if (isSameAsPopUpped) 
                        _popupLevelObjectId = -1;
                    else 
                        _popupLevelObjectId = i;
                }
                bool selected = IsSelected(obj.visual.gameObject);
                if (GUILayout.Button("Select", selected ? _settingsEditor.buttonBlue : _settingsEditor.buttonGray, GUILayout.Width(90)))
                    Selection.objects = new UnityEngine.Object[] { obj.visual };
                if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(30)))
                {
                    if (EditorUtility.DisplayDialog("Delete", "Are you sure that you want to delete this object?", "Yes", "Cancel"))
                    {
                        DestroyImmediate(obj.visual.gameObject);
                        objects.DeleteArrayElementAtIndex(i);
                        Save();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (isSameAsPopUpped)
                {
                    SerializedProperty position = objSerialised.FindPropertyRelative("position");
                    SerializedProperty eulerAngles = objSerialised.FindPropertyRelative("eulerAngles");
                    SerializedProperty localScale = objSerialised.FindPropertyRelative("localScale");
                    EditorGUILayout.PropertyField(position);
                    EditorGUILayout.PropertyField(eulerAngles);
                    EditorGUILayout.PropertyField(localScale);
                }
                EditorGUILayout.Space(2);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space(5);

            //Last object
            if (levelObjects.Count > 0)
            {
                LevelObject lastObject = levelObjects[^1];
                if (lastObject != null)
                {
                    GUILayout.Label("Last added object", _settingsEditor.labelTitle);
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(lastObject.prefab.name);
                    var selected = IsSelected(lastObject.visual.gameObject);
                    if (GUILayout.Button("Select", selected ? _settingsEditor.buttonBlue : _settingsEditor.buttonGray, GUILayout.Width(120)))
                        Selection.objects = new UnityEngine.Object[] { lastObject.visual };
                    EditorGUILayout.EndHorizontal();
                }
            }

            GUILayout.Label("Prefabs selector", _settingsEditor.labelTitle);
            _scrollPositionModifyPrefabsSelector = EditorGUILayout.BeginScrollView(_scrollPositionModifyPrefabsSelector, GUILayout.Height(240));
            DrawGamePrefabButtonsLine("Basic", _gamePrefabDataBasic);
            DrawGamePrefabButtonsLine("Grounds", _gamePrefabDataGrounds);
            DrawGamePrefabButtonsLine("Other", _gamePrefabDataOther);
            EditorGUILayout.EndScrollView();
            Repaint();
        }
        
        private void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _settingsEditor.buttonGray, GUILayout.Width(80)))
            {
                GUI.FocusControl(null);
                ChangeTypeTo(Type.Main);
            }
            GUILayout.Label("Settings", _settingsEditor.labelTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(15);

            GUILayout.Label("Set new object to");
            _levelEditorSetNewObjectToPreviousPosition.boolValue = EditorGUILayout.Toggle("Previous position: ", _levelEditorSetNewObjectToPreviousPosition.boolValue);
            _levelEditorSetNewObjectToPreviousRotation.boolValue = EditorGUILayout.Toggle("Previous rotation: ", _levelEditorSetNewObjectToPreviousRotation.boolValue);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Unlock All Levels", _settingsEditor.buttonRed, CustomGUILayout.ButtonCentral()))
            {
                CustomPlayerPrefs.SetInt("levelsUnlockedID", int.MaxValue);
                Debug.Log("Level editor: All levels unlocked!");
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Lock All Levels", _settingsEditor.buttonYellow, CustomGUILayout.ButtonCentral()))
            {
                CustomPlayerPrefs.SetInt("levelsUnlockedID", 0);
                Debug.Log("Level editor: All levels locked!");
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawLevelObjects()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _settingsEditor.buttonGray, GUILayout.Width(80)))
            {
                GUI.FocusControl(null);
                ChangeTypeTo(Type.Main);
            }
            GUILayout.Label("Level Objects", _settingsEditor.labelTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            _toolbarLevelObjectsTypeSelected = GUILayout.Toolbar(_toolbarLevelObjectsTypeSelected, _toolbarLevelObjectsType, _settingsEditor.toolbarSmall, CustomGUILayout.ToolBar());
            EditorGUILayout.Space(5);
            switch (_toolbarLevelObjectsTypeSelected)
            {
                case 0:
                    GUILayout.Label("Basic", _settingsEditor.labelTitle);
                    DrawLevelObjectsArray(_gamePrefabDataBasic);
                    break;
                case 1:
                    GUILayout.Label("Grounds", _settingsEditor.labelTitle);
                    DrawLevelObjectsArray(_gamePrefabDataGrounds);
                    break;
                case 2:
                    GUILayout.Label("Other", _settingsEditor.labelTitle);
                    DrawLevelObjectsArray(_gamePrefabDataOther);
                    break;
            }
        }
        
        private void DrawLevelObjectsArray(SerializedProperty gamePrefab)
        {
             _scrollPositionMain = EditorGUILayout.BeginScrollView(_scrollPositionMain);
            for (int i = 0; i < gamePrefab.arraySize; i++)
            {
                var dataElement = gamePrefab.GetArrayElementAtIndex(i);
                var name = dataElement.FindPropertyRelative("name");

                if (i % 2 == 0) 
                    Separator(31);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name.stringValue, EditorStyles.boldLabel);

                if (GUILayout.Button("Edit", _settingsEditor.buttonYellow, GUILayout.Width(70)))
                {
                    _selectedLevelObject = dataElement;
                    _selectedLevelObjectId = i;
                    ChangeTypeTo(Type.SelectedLevelObject);
                }
                GUILayout.Space(15);
                if (gamePrefab.arraySize > 1)
                {
                    if (i != 0)
                    {
                        if (GUILayout.Button("↑", _settingsEditor.buttonGray, GUILayout.Width(26))) gamePrefab.MoveArrayElement(i, i - 1);
                    }
                    else GUILayout.Space(29);
                    if (i != gamePrefab.arraySize - 1)
                    {
                        if (GUILayout.Button("↓", _settingsEditor.buttonGray, GUILayout.Width(26))) gamePrefab.MoveArrayElement(i, i + 1);
                    }
                    else GUILayout.Space(29);
                }
                GUILayout.Space(15);
                if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                {
                    if (EditorUtility.DisplayDialog("Delete", "Are you sure that you want to delete this level?", "Yes", "Cancel"))
                    {
                        gamePrefab.DeleteArrayElementAtIndex(i);
                        _soGamePrefabHolder.ApplyModifiedProperties();
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(2);
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Create New", _settingsEditor.buttonGreen, CustomGUILayout.ButtonLarge()))
            {
                gamePrefab.arraySize++;
                gamePrefab.serializedObject.ApplyModifiedProperties();

                var newElement = gamePrefab.GetArrayElementAtIndex(gamePrefab.arraySize - 1);
                var name = newElement.FindPropertyRelative("name");
                var icon = newElement.FindPropertyRelative("icon");
                var prefab = newElement.FindPropertyRelative("prefab");

                name.stringValue = "";
                icon.objectReferenceValue = null;
                prefab.objectReferenceValue = null;

                gamePrefab.serializedObject.ApplyModifiedProperties();
                _soGamePrefabHolder.ApplyModifiedProperties();
            }
        }
        
        private void DrawSelectedLevelObjects()
        {
            SerializedProperty name = _selectedLevelObject.FindPropertyRelative("name");
            SerializedProperty icon = _selectedLevelObject.FindPropertyRelative("icon");
            SerializedProperty prefab = _selectedLevelObject.FindPropertyRelative("prefab");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _settingsEditor.buttonGray, GUILayout.Width(80)))
            {
                GUI.FocusControl(null);
                ChangeTypeTo(Type.LevelObjects);
            }
            GUILayout.Label("Level Object - " + name.stringValue, _settingsEditor.labelTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(15);

            EditorGUILayout.PropertyField(name);
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(icon);
            EditorGUILayout.PropertyField(prefab);

            _selectedLevelObject.serializedObject.ApplyModifiedProperties();
            _soGamePrefabHolder.ApplyModifiedProperties();
        }
        
        private void CreateNewLevel()
        {
            _levelHolderDataProperty.arraySize++;
            _levelHolderDataProperty.serializedObject.ApplyModifiedProperties();
            ClearLevel(_levelHolderDataProperty.arraySize - 1);
        }
        
        private void ClearLevel(int id)
        {
            SerializedProperty newElement = _levelHolderDataProperty.GetArrayElementAtIndex(id);
            SerializedProperty name = newElement.FindPropertyRelative("name");
            SerializedProperty levelId = newElement.FindPropertyRelative("id");
            SerializedProperty levels = newElement.FindPropertyRelative("objects");
            SerializedProperty sawSpeed = newElement.FindPropertyRelative("sawSpeed");

            name.stringValue = "";
            levelId.stringValue = Guid.NewGuid().ToString("N");
            sawSpeed.floatValue = 5;
            levels.ClearArray();

            _soLevelHolder.ApplyModifiedProperties();

            Debug.Log("Level editor: Level cleared");
        }
        
        private void Save()
        {
            if (LevelEditorPreview.Instance != null)
                LevelEditorPreview.Instance.Save();
        }
        
        private void DrawGamePrefabButtonsLine(string sectionName, SerializedProperty data)
        {
            if (data.arraySize > 0)
            {
                GUILayout.Label(sectionName);
                var maxButtonsAtLine = 5;
                var currentButtonsAtLine = maxButtonsAtLine;
                var isBeginHorizontal = false;
                for (int i = 0; i < data.arraySize; i++)
                {
                    if (currentButtonsAtLine == maxButtonsAtLine)
                    {
                        isBeginHorizontal = true;
                        EditorGUILayout.BeginHorizontal();
                    }
                    currentButtonsAtLine--;

                    var element = data.GetArrayElementAtIndex(i);
                    var name = element.FindPropertyRelative("name").stringValue;
                    var icon = element.FindPropertyRelative("icon");
                    var prefab = element.FindPropertyRelative("prefab");
                    var iconAddress = AssetDatabase.GetAssetPath(icon.objectReferenceValue);
                    var prefabAddress = AssetDatabase.GetAssetPath(prefab.objectReferenceValue);

                    if (GUILayout.Button(CustomGUIContent.GetImage(iconAddress, name), _settingsEditor.buttonBlack))
                    {
                        if (LevelEditorPreview.Instance != null)
                            LevelEditorPreview.Instance.Save();

                        _levelHolderLoaded.AddGamePrefab(prefabAddress, _selectedLevelId);
                        if (LevelEditorPreview.Instance != null)
                        {
                            LevelEditorPreview.Instance.DrawObjects();
                            LevelEditorPreview.Instance.SetActiveLatestObject();
                        }

                        Debug.Log("Level editor: New object of field, " + name);
                    }

                    if (currentButtonsAtLine == 0)
                    {
                        currentButtonsAtLine = maxButtonsAtLine;
                        isBeginHorizontal = false;
                        EditorGUILayout.EndHorizontal();
                    }
                }
                if (isBeginHorizontal) EditorGUILayout.EndHorizontal();
            }
        }
        
        private void ChangeTypeTo(Type type)
        {
            if (type == Type.Specific || type == Type.Modify)
            {
                EditorSceneManager.OpenScene("Assets/Scenes/Editor/LevelEditorScene.scene");
                if (LevelEditorPreview.Instance != null)
                    LevelEditorPreview.Instance.Load(_selectedLevelId);
            }
            else
            {
                if (LevelEditorPreview.Instance != null)
                    LevelEditorPreview.Instance.Close();
            }
            EditorUtility.SetDirty(_levelHolderLoaded);
            _currentType = type;
            SceneView.RepaintAll();
        }
        
        private void Separator(int height)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = height;
            EditorGUI.DrawRect(rect, _settingsEditor.backgroundFrameColor);
        }
        
        private bool IsSelected(GameObject obj)
        {
            foreach (var selectedObj in Selection.objects)
                if (selectedObj == obj) 
                    return true;
            
            return false;
        }
    }
}
#endif