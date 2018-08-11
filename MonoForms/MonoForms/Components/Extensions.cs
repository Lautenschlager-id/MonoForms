using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoForms
{
    static class Extensions
    {
        public static decimal Clamp(this decimal value, decimal min, decimal max)
        {
            return Math.Min(max, Math.Max(value, min));
        }

        public static float Clamp(this float value, float min, float max)
        {
            return Math.Min(max, Math.Max(value, min));
        }

        public static int Clamp(this int value, int min, int max)
        {
            return Math.Min(max, Math.Max(value, min));
        }

        public static void Fill(this Texture2D texture)
        {
            texture.Fill(texture.Width * texture.Height);
        }

        public static void Fill(this Texture2D texture, int dataLegth)
        {
            Color[] data = new Color[dataLegth];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            texture.SetData(data);
        }

        public static string Repeat(this string str, int quantity)
        {
            if (quantity == 0) return string.Empty;

            string ret = str;
            for (int i = 1; i < quantity; i++)
                ret += str;
            return ret;
        }

        public static Vector2 Size(this Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height);
        }
    }
}
