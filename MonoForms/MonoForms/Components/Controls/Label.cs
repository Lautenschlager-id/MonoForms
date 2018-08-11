using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace MonoForms
{
    class Label : Control
    {
        #region Property
        public override bool AutoEllipsis
        {
            get => autoEllipsis;
            set
            {
                autoEllipsis = value;

                // Triggers the Text_set_ to reformat the string
                Text = Text;
            }
        }

        public override bool AutoSize
        {
            set
            {
                autoSize = value;

                // Triggers the Text_set_ to reformat the string
                Text = Text;
            }
        }

        public override MonoSpriteFont Font
        {
            set
            {
                base.Font = value;

                // Triggers the Text_set_ to math the size of the new font
                Text = Text;
            }
        }

        public override string Text
        {
            set
            {
                if (value == null) return;

                base.Text = value;

                // Defines the _displayedText_
                string text = string.Empty;

                Vector2 textSize = Vector2.One * 10; // tolerance

                char[] characters = value.ToCharArray();

                if (AutoSize)
                {
                    if (GrowVertically) // fixed width
                    {
                        for (int i = 0; i < characters.Length; i++)
                            if (FontData.Collection[Font.Id].ContainsKey(characters[i]))
                            {
                                Vector2 charSize = FontData.Collection[Font.Id][characters[i]];

                                float widthSum = textSize.X + charSize.X;

                                string charString = characters[i].ToString();

                                if (widthSum < Size.X * .9f) // tolerance
                                {
                                    textSize += charSize;
                                    text += charString;
                                }
                                else
                                {
                                    text += "\n" + (charString == " " ? string.Empty : charString); // trim "^ "
                                    textSize.X = 0;
                                }
                            }

                        Size = new Vector2(Size.X, Vector2.Max(Font.SpriteFont.MeasureString(text), Vector2.One).Y);
                    }
                    else
                        Size = Vector2.Max(Font.SpriteFont.MeasureString(text = value), Vector2.One);
                }
                else
                {
                    bool broke = false;

                    if (RightContent)
                    {
                        for (int i = characters.Length - 1; i >= 0; i--)
                            if (FontData.Collection[Font.Id].ContainsKey(characters[i]))
                            {
                                Vector2 charSize = FontData.Collection[Font.Id][characters[i]];

                                float widthSum = textSize.X + charSize.X;

                                if (widthSum < Size.X)
                                {
                                    textSize += charSize;
                                    text = characters[i].ToString() + text; // right to left
                                }
                                else
                                {
                                    broke = true;
                                    break;
                                }
                            }

                        if (broke && AutoEllipsis)
                            text = Regex.Replace(text, "^...", "...");
                    }
                    else
                    {
                        for (int i = 0; i < characters.Length; i++)
                            if (FontData.Collection[Font.Id].ContainsKey(characters[i]))
                            {
                                Vector2 charSize = FontData.Collection[Font.Id][characters[i]];

                                float widthSum = textSize.X + charSize.X;

                                if (widthSum < Size.X)
                                {
                                    textSize += charSize;
                                    text += characters[i].ToString(); // left to right
                                }
                                else
                                {
                                    broke = true;
                                    break;
                                }
                            }

                        if (broke && AutoEllipsis)
                            text = Regex.Replace(text, "...$", "...");
                    }
                }

                DisplayedText = text;
            }
        }
        #endregion

        #region PrivateProperty
        protected bool autoEllipsis; // avoids triggering the text normalization in the constructor
        #endregion

        #region ControlProperty
        public bool GrowVertically
        {
            get => growVertically;
            set
            {
                growVertically = value;

                // Triggers the Text_set_ to reformat the string
                Text = Text;
            }
        }
        
        public bool RightContent
        {
            get => rightContent;
            set
            {
                rightContent = value;

                // Triggers the Text_set_ to reformat the string
                Text = Text;
            }
        }
        #endregion

        #region ControlPrivateProperty
        public string DisplayedText { get; set; }

        protected bool growVertically;

        protected bool rightContent;
        #endregion

        public Label()
        {
            #region Properties
            autoEllipsis = false;
            autoSize = false;
            BackColor = Color.Transparent;
            DisplayedText = string.Empty;
            font = Game.DefaultFont;
            ForeColor = Color.Black;
            growVertically = false;
            rightContent = false;
            #endregion

            #region Events
            Paint += eventPaint;
            #endregion
        }

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (DisplayedText != string.Empty)
                e.SpriteBatch.DrawString(Font.SpriteFont, DisplayedText, Location, ForeColor * Alpha);
        }
        #endregion
    }
}
