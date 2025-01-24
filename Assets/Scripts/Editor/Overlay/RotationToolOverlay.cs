#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Overlay
{
    [Overlay(typeof(SceneView), "RunningCar - Rotation Tool", true)]
    public class RotationToolOverlay : UnityEditor.Overlays.Overlay
    {
        public bool rotateGroup;
        
        public override VisualElement CreatePanelContent()
        {
            VisualElement root = new()
            {
                name = "Rotation Tool",
                style =
                {
                    flexDirection = FlexDirection.Row
                }
            };

            RotationToolOverlay_ButtonRotate button1 = new("\u21bb", -90, RotationToolOverlay_ButtonType.Left, this);
            RotationToolOverlay_ButtonRotate button2 = new("\u21a9", 180, RotationToolOverlay_ButtonType.Central, this);
            RotationToolOverlay_ButtonRotate button3 = new("\u21ba", -90, RotationToolOverlay_ButtonType.Right, this);
            root.Add(button1);
            root.Add(button2);
            root.Add(button3);

            Toggle toggleButton = new Toggle("Rotate Group")
            {
                value = rotateGroup
            };
            toggleButton.RegisterValueChangedCallback(_ => { rotateGroup = !rotateGroup; });
            toggleButton.style.marginTop = 5;
            root.Add(toggleButton);
            
            return root;
        }
    }


    public enum RotationToolOverlay_ButtonType
    {
        Left, Central, Right
    }
    
    [EditorToolbarElement(id, typeof(SceneView))]
    public class RotationToolOverlay_ButtonRotate : EditorToolbarButton
    {
        public RotationToolOverlay specificOverlay;
        public const string id = "RunningCar - Toolbar/Button";
        private float buttonSize = 35;
        private float angle;

        public RotationToolOverlay_ButtonRotate(string text, float angle, RotationToolOverlay_ButtonType type, RotationToolOverlay specificOverlay)
        {
            this.specificOverlay = specificOverlay;
            this.angle = angle;
            this.text = text;

            // Size buttons
            style.width = buttonSize;
            style.height = buttonSize;

            style.marginLeft = 1;
            style.marginRight = 1;
            style.marginTop = 3;
            style.marginBottom = 3;
            if (type == RotationToolOverlay_ButtonType.Left)
            {
                style.marginLeft = 3;
                style.borderTopLeftRadius = 5;
                style.borderBottomLeftRadius = 5;
            }
            if (type == RotationToolOverlay_ButtonType.Right)
            {
                style.marginRight = 3;
                style.borderTopRightRadius = 5;
                style.borderBottomRightRadius = 5;
            }
            
            // Text
            style.justifyContent = Justify.Center;
            style.alignItems = Align.Center;
            style.unityTextAlign = TextAnchor.MiddleCenter;
            
            // Color
            if (type != RotationToolOverlay_ButtonType.Central)
                style.backgroundColor = Color.gray;
            style.color = Color.white;
            
            // Actions
            clicked += OnClick;
        }

        void OnClick()
        {
            if(specificOverlay.rotateGroup)
                RotateGroup();
            else
                RotateIndividual();
        }

        private void RotateIndividual()
        {
            foreach (Transform selected in Selection.transforms)
                selected.eulerAngles = new Vector3(selected.eulerAngles.x, selected.eulerAngles.y + angle, selected.eulerAngles.z);
        }

        private void RotateGroup()
        {
            Transform[] transforms = Selection.transforms;
            if (transforms.Length == 0)
                return;

            Vector3 center = Vector3.zero;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Get center point
            foreach (Transform trans in transforms)
                center += trans.position;
            center /= transforms.Length;
            
            // Apply position and rotations
            foreach (Transform trans in transforms)
            {
                Vector3 localPosition = trans.position - center;
                localPosition = rotation * localPosition;
                trans.position = center + localPosition;
                trans.rotation = rotation * trans.rotation;
            }
        }
    }
}
#endif