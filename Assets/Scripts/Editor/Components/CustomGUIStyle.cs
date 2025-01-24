#if UNITY_EDITOR
using Interface.Components;
using UnityEditor;
using UnityEngine;

namespace Editor.Components
{
    public static class CustomGUIStyle
    {
        public static GUIStyle Box(Color color)
        {
            GUIStyle style = new(GUI.skin.box)
            {
                normal =
                {
                    background = CustomTexture2D.Make(color)
                }
            };
            return style;
        }
    }

    public static class CustomGUIContent
    {
        public static GUIContent GetImage(string address, string text)
        {
            Texture2D image = AssetDatabase.LoadAssetAtPath<Texture2D>(address);
            GUIContent content = new()
            {
                image = image,
                text = text
            };
            return content;
        }

        public static GUIContent GetImage(Texture2D texture, string text)
        {
            return new GUIContent
            {
                image = texture,
                text = text
            };
        }
    }
}
#endif