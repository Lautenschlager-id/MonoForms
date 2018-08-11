using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MonoForms
{
    static class Tools
    {
        private static int fontId = -1;

        public static MonoSpriteFont GenerateSpriteFont(string fontName, float size, List<Point> characterRegions, float spacing = 0, bool useKerning = true, string style = "Regular", char defaultCharacter = '?')
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
            "<XnaContent xmlns:Graphics=\"Microsoft.Xna.Framework.Content.Pipeline.Graphics\">\r\n" +
            "\t<Asset Type=\"Graphics:FontDescription\">\r\n" +

            $"\t\t<FontName>{fontName}</FontName>\r\n" +
            $"\t\t<Size>{size.ToString(CultureInfo.InvariantCulture)}</Size>\r\n" +
            $"\t\t<Spacing>{spacing.ToString(CultureInfo.InvariantCulture)}</Spacing>\r\n" +
            $"\t\t<UseKerning>{useKerning.ToString().ToLower()}</UseKerning>\r\n" +
            $"\t\t<Style>{style}</Style>\r\n\r\n" +

            $"\t\t<DefaultCharacter>{defaultCharacter}</DefaultCharacter>\r\n\r\n" +

            "\t\t<CharacterRegions>\r\n";
            foreach (Point region in characterRegions)
            {
                xml += "\t\t\t<CharacterRegion>\r\n" +
                $"\t\t\t\t<Start>&#{region.X};</Start>\r\n" +
                $"\t\t\t\t<End>&#{region.Y};</End>\r\n" +
                "\t\t\t</CharacterRegion>\r\n";
            }
            xml += "\t\t</CharacterRegions>\r\n" +
            "\t</Asset>\r\n" +
            "</XnaContent>";

            SpriteFont spriteFont;
            try
            {
                spriteFont = Game.Instance.Content.Load<SpriteFont>("Fonts/" + fontId);
            }
            catch (Microsoft.Xna.Framework.Content.ContentLoadException)
            {
                throw new FileNotFoundException($"Create a file in the folder Fonts called {fontId}.spritefont with the XML below:\n\n{xml}\n\n");
            }

            //using (TextWriter manager = new StreamWriter("Content/Fonts/" + fontId + ".spritefont"))
            //    manager.WriteLine(xml);

            MonoSpriteFont generated = new MonoSpriteFont()
            {
                Id = fontId,
                SpriteFont = spriteFont
            };

            FontData.Fonts.Add(generated);

            FontData.Collection.Add(fontId, new Dictionary<char, Vector2>());
            foreach (Point region in characterRegions)
                for (int i = region.X; i <= region.Y; i++)
                {
                    char c = Convert.ToChar(i);
                    FontData.Collection[fontId].Add(c, generated.SpriteFont.MeasureString(c.ToString()));
                }

            fontId++;
            return generated;
        }
    }
}
