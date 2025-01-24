using UnityEngine;

namespace Interface.Components
{
    public static class CustomTexture2D
    {
        public static Texture2D Make(Color color)
        {
            Color[] pixels = new Color[4]{color, color, color, color};
            Texture2D texture = new Texture2D(2, 2);
            texture.SetPixels(pixels);
            texture.Apply();
            
            return texture;
        }

        public static Texture2D MakeRounded(Color color, int radius)
        {
            int diameter = radius * 2;
            Texture2D texture = new(diameter, diameter);
            Color[] pixels = new Color[diameter * diameter];

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius));
                    pixels[y * diameter + x] = dist <= radius ? color : Color.clear;
                }
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}
