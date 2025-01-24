#if UNITY_EDITOR
using UnityEditor;

namespace Interface.Components
{
    [CustomEditor(typeof(NestedScrollRect))]
    [CanEditMultipleObjects]
    public class NestedScrollRectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
#endif