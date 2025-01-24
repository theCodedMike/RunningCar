#if UNITY_EDITOR
using System;
using System.Utility;
using Editor.Components;
using ScriptableObjects.Settings;
using ScriptableObjects.Skins;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SkinHolderEditor : EditorWindow
    {
        private enum Type {Main, Selected, Options}

        private Vector2 _scrollPositionMain;
        private Vector2 _scrollPositionSelected;
        private Type _currentType = Type.Main;

        private int _toolbarSkinTypeSelected;
        private readonly string[] _toolbarSkinType = { "Body", "Wheels" };
        
        private static SkinHolder _skinHolder;
        private static SettingsEditor _settingsEditor;
        private const string AssetAddress = "Assets/ScriptableObjects/Skins/SkinHolder.asset";
        private const string SettingsEditorAddress = "Assets/ScriptableObjects/Settings/SettingsEditor.asset";
        private static SerializedObject _soSkinHolder;
        
        // Values
        private static SerializedProperty _skinsBody;
        private static SerializedProperty _skinsWheels;
        private static SerializedProperty _selectedSkin;
        private static int _selectedSkinId;
        
        
        
        
        [MenuItem("Tools/RunningCar/Skins", false, 21)]
        public static void ShowWindow()
        {
            GetWindow<SkinHolderEditor>("RunningCar - Skins");
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            _skinHolder = AssetDatabase.LoadAssetAtPath<SkinHolder>(AssetAddress);
            _settingsEditor = AssetDatabase.LoadAssetAtPath<SettingsEditor>(SettingsEditorAddress);

            _soSkinHolder = new SerializedObject(_skinHolder);
            _soSkinHolder.Update();

            _skinsBody = _soSkinHolder.FindProperty("skinsBody");
            _skinsWheels = _soSkinHolder.FindProperty("skinsWheels");
        }

        void OnGUI()
        {
            EditorGUILayout.Space(10);

            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, CustomGUIStyle.Box(_settingsEditor.backgroundColor));

            switch (_currentType)
            {
                case Type.Main:
                    DrawMain();
                    break;
                case Type.Selected:
                    DrawSelected();
                    break;
                case Type.Options:
                    DrawOptions();
                    break;
            }

            _soSkinHolder.ApplyModifiedProperties();
        }

        void DrawMain()
        {
            _toolbarSkinTypeSelected = GUILayout.Toolbar(_toolbarSkinTypeSelected, _toolbarSkinType, _settingsEditor.toolbarMain, CustomGUILayout.ToolBar());
            EditorGUILayout.Space(10);
            switch (_toolbarSkinTypeSelected)
            {
                case 0:
                    GUILayout.Label("Body", _settingsEditor.labelTitle);
                    EditorGUILayout.Space(5);
                    DrawArraySkins(_skinsBody);
                    break;
                default:
                    GUILayout.Label("Wheels", _settingsEditor.labelTitle);
                    EditorGUILayout.Space(5);
                    DrawArraySkins(_skinsWheels);
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create New", _settingsEditor.buttonGreen, CustomGUILayout.ButtonLarge()))
            {
                if (_toolbarSkinTypeSelected == 0) 
                    CreateNewSkinBody();
                else 
                    CreateNewSkinWheels();
            }
            if (GUILayout.Button("Options", _settingsEditor.buttonGray, CustomGUILayout.ButtonLarge(80))) 
                ChangeTypeTo(Type.Options);
            EditorGUILayout.EndHorizontal();

            _soSkinHolder.ApplyModifiedProperties();
        }

        void DrawSelected()
        {
            if (_selectedSkin == null)
            {
                ChangeTypeTo(Type.Main);
                return;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _settingsEditor.buttonGray, GUILayout.Width(80)))
            {
                GUI.FocusControl(null);
                ChangeTypeTo(Type.Main);
            }
            GUILayout.Label($"Skin {_selectedSkinId + 1}", _settingsEditor.labelTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            SerializedProperty name = _selectedSkin.FindPropertyRelative("name");
            SerializedProperty id = _selectedSkin.FindPropertyRelative("id");
            SerializedProperty price = _selectedSkin.FindPropertyRelative("price");
            SerializedProperty mesh = _selectedSkin.FindPropertyRelative("mesh");
            SerializedProperty material = _selectedSkin.FindPropertyRelative("material");

            SerializedProperty speed = _selectedSkin.FindPropertyRelative("speed");
            SerializedProperty steering = _selectedSkin.FindPropertyRelative("steering");
            SerializedProperty velocity = _selectedSkin.FindPropertyRelative("velocity");

            _scrollPositionSelected = EditorGUILayout.BeginScrollView(_scrollPositionSelected);

            EditorGUILayout.PropertyField(name);
            EditorGUILayout.Space(5);
            GUILayout.Label("ID: " + id.stringValue);
            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(price);
            EditorGUILayout.Space(5);

            EditorGUILayout.ObjectField(mesh);
            EditorGUILayout.ObjectField(material);

            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(steering);
            EditorGUILayout.PropertyField(velocity);

            EditorGUILayout.EndScrollView();
        }

        void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _settingsEditor.buttonGray, CustomGUILayout.ButtonBack()))
            {
                GUI.FocusControl(null);
                ChangeTypeTo(Type.Main);
            }
            GUILayout.Label($"Options", _settingsEditor.labelTitle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Unlock All Skins", _settingsEditor.buttonRed, CustomGUILayout.ButtonCentral())) 
                _skinHolder.UnlockAll();
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Lock All Skins", _settingsEditor.buttonYellow, CustomGUILayout.ButtonCentral())) 
                _skinHolder.LockAll();
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(25);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Get 500 Coins", _settingsEditor.buttonGreen, CustomGUILayout.ButtonCentral()))
            {
                var coin = CustomPlayerPrefs.GetInt("coin") + 500;
                CustomPlayerPrefs.SetInt("coin", coin);
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        void ChangeTypeTo(Type type)
        {
            _currentType = type;
            SceneView.RepaintAll();
        }

        #region Work with skins

        void DrawArraySkins(SerializedProperty skins)
        {
            EditorGUILayout.BeginVertical();
            if (_skinHolder != null && _soSkinHolder != null && skins != null)
            {
                _soSkinHolder.Update();
                _scrollPositionMain = EditorGUILayout.BeginScrollView(_scrollPositionMain);
                for (int i = 0; i < skins.arraySize; i++)
                {
                    SerializedProperty dataElement = skins.GetArrayElementAtIndex(i);
                    SerializedProperty name = dataElement.FindPropertyRelative("name");

                    if (i % 2 == 0)
                        Separator(31);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Skin {i + 1} - {name.stringValue}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Edit", _settingsEditor.buttonYellow, GUILayout.Width(70)))
                    {
                        _selectedSkin = dataElement;
                        _selectedSkinId = i;
                        ChangeTypeTo(Type.Selected);
                    }
                    GUILayout.Space(15);
                    if (skins.arraySize > 1)
                    {
                        if (i != 0)
                        {
                            if (GUILayout.Button("↑", _settingsEditor.buttonGray, GUILayout.Width(26))) 
                                skins.MoveArrayElement(i, i - 1);
                        }
                        else
                            GUILayout.Space(29);
                        if (i != skins.arraySize - 1)
                        {
                            if (GUILayout.Button("↓", _settingsEditor.buttonGray, GUILayout.Width(26))) 
                                skins.MoveArrayElement(i, i + 1);
                        }
                        else
                            GUILayout.Space(29);
                    }
                    GUILayout.Space(15);
                    if (GUILayout.Button("-", _settingsEditor.buttonRed, GUILayout.Width(26)))
                    {
                        if (EditorUtility.DisplayDialog("Delete", "Are you sure that you want to delete this skin?", "Yes", "Cancel"))
                            skins.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        void CreateNewSkinBody()
        {
            CreateNew(_skinsBody, true);
        }

        void CreateNewSkinWheels()
        {
            CreateNew(_skinsWheels, true);
        }

        void CreateNew(SerializedProperty skins, bool isSkinBody)
        {
            skins.arraySize++;

            SerializedProperty newElement = skins.GetArrayElementAtIndex(skins.arraySize - 1);
            SerializedProperty name = newElement.FindPropertyRelative("name");
            SerializedProperty id = newElement.FindPropertyRelative("id");
            SerializedProperty price = newElement.FindPropertyRelative("price");
            SerializedProperty mesh = newElement.FindPropertyRelative("mesh");
            SerializedProperty material = newElement.FindPropertyRelative("material");

            if (skins.arraySize - 1 == 0)
            {
                if (isSkinBody) 
                    _skinHolder.skinsBody[skins.arraySize - 1].Unlock();
                else 
                    _skinHolder.skinsWheels[skins.arraySize - 1].Unlock();
            }

            name.stringValue = "";
            id.stringValue = Guid.NewGuid().ToString("N");
            price.intValue = 0;
            mesh.objectReferenceValue = null;
            material.objectReferenceValue = null;

            skins.serializedObject.ApplyModifiedProperties();
        }

        private void Separator(int height)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = height;
            EditorGUI.DrawRect(rect, _settingsEditor.backgroundFrameColor);
        }

        #endregion
    }
}
#endif