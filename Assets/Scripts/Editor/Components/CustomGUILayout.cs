#if UNITY_EDITOR
using UnityEngine;

namespace Editor.Components
{
    public static class CustomGUILayout
    {
        #region Toolbar
        public static GUILayoutOption ToolBar() 
            => GUILayout.Height(40);
        
        public static GUILayoutOption ToolbarSmall() 
            => GUILayout.Height(28);
        #endregion

        #region Buttons
        public static GUILayoutOption ButtonLarge() 
            => GUILayout.Height(35);

        public static GUILayoutOption[] ButtonLarge(float width) 
            => new[] { GUILayout.Width(width), GUILayout.Height(35) };
        
        public static GUILayoutOption[] ButtonBack() 
            => new[] { GUILayout.Width(75), GUILayout.Height(25) };

        public static GUILayoutOption[] ButtonCentral()
            => new[] { GUILayout.Width(145), GUILayout.Height(30) };
        #endregion
    }
}
#endif