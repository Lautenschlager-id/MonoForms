using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoForms
{
    static class FontData
    {
        public static List<MonoSpriteFont> Fonts = new List<MonoSpriteFont>();

        public static Dictionary<int, Dictionary<char, Vector2>> Collection = new Dictionary<int, Dictionary<char, Vector2>>();
    }
}
