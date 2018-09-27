using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoForms
{
    class TextBox : Control
    {
        /* TODO
         * ctrl c - another handler
         * ctrl x
         * ctrl a
         * select
         * shift + < or shift + > to select [ if square is going < then <=> >=<]
         */

        #region Property
        public override MonoSpriteFont Font
        {
            get => label.Font;
            set
            {
                label.Font = value;

                if (pointerDisplay != null)
                    pointerDisplay.Font = value;

                if (selectedText != null)
                    selectedText.Font = value;
            }
        }

        public override Vector2 Location
        {
            set
            {
                if (selectedText != null)
                    selectedText.Location = base.Location - (base.Location - value);

                base.Location = value;

                if (label != null)
                    label.Location = value;

                if (pointerDisplay != null)
                    pointerDisplay.Location = value;
            }
        }

        public override bool Selected
        {
            set
            {
                base.Selected = value;

                if (!value)
                    if (pointerDisplay != null)
                        pointerDisplay.Text = "";
            }
        }

        public override Vector2 Size
        {
            set
            {
                base.Size = value;

                if (label != null)
                    label.Size = base.Size;

                if (pointerDisplay != null)
                    pointerDisplay.Size = base.Size;
            }
        }

        public override string Text
        {
            get => label.Text;
            set
            {
                label.Text = value;

                if (history.Count == 0 || value != history[Math.Max(0, history.Count - 2)])
                    history.Add(value);

                int size = Math.Max(0, label.Text.Length - 1);
                if (pointerLocale == 0 || pointerLocale + 1 == size)
                    movePointer(1);

                TextChanged?.Invoke(this, new MonoValueEventArg(label.Text));

                textToPassword(); // there's an if inside the method
            }
        }
        #endregion

        #region Control
        public bool AcceptsTab { get; set; }

        public Keys[] AvailableKeys { get; set; }

        public Keys[] DisabledKeys { get; set; }

        public bool HideSelection { get; set; } // When focus another shit, if the text blue selection must disappear

        public int InputHoldingTime { get; set; }

        public string[] Lines
        {
            get => label.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public int MaxLength { get; set; }

        public bool Multiline { get; set; }

        public char PasswordChar { get; set; }

        public int PointerLine
        {
            get => pointerLine;
            set
            {
                pointerLine = value.Clamp(0, Lines.Length - 1);
                label.DisplayedText = Lines[PointerLine];
            }
        }

        public int PointerLocale
        {
            get => lineLength() + pointerLocale;
            set => pointerLocale = value.Clamp(0, Math.Max(Lines[pointerLine].Length - 1, 0));
        }

        public int PointerSpeed
        {
            get => pointerSpeed;
            set
            {
                defaultPointerSpeed = pointerSpeed = value;
            }
        }

        public bool ReadOnly { get; set; }

        public Color SelectionColor { get; set; }

        public bool UseSystemPassword { get; set; }
        #endregion

        #region ControlPrivateProperty
        private int defaultPointerSpeed;

        private Dictionary<Keys, float> holdingKeys = new Dictionary<Keys, float>();

        private List<string> history = new List<string>();

        private List<string> redoHistory = new List<string>();

        private Keys[] holdingKeysList
        {
            get => holdingKeys.Keys.ToArray();
        }

        private string[] keyboardNumberCtrlAlt = { string.Empty, "¹", "²", "³", "£", "¢", "¬", string.Empty, string.Empty, string.Empty };

        private string[] keyboardNumberShift = { ")", "!", "@", "#", "$", "%", "¨", "&", "*", "(" };

        private Keys[] keyboardSpecialKeys = { Keys.LeftShift, Keys.RightShift, Keys.LeftControl, Keys.RightControl, Keys.LeftAlt, Keys.RightAlt, Keys.CapsLock };

        private Label label, pointerDisplay;

        private int pointerLine, pointerLocale, pointerSpeed;

        private Label selectedText;
        #endregion

        #region Event
        public event EventHandler<MonoValueEventArg> TextChanged;
        #endregion

        public TextBox()
        {
            #region Control
            label = new Label()
            {
                RightContent = true,
                Text = ""
            };

            pointerDisplay = new Label()
            {
                RightContent = true,
                Text = ""
            };

            selectedText = new Label()
            {
                ForeColor = Color.Transparent,
                Size = Vector2.One,
                Text = "",
            };
            #endregion

            #region Property
            AcceptsTab = false;
            AvailableKeys = new Keys[] { };
            BackColor = Color.GhostWhite * .6f;
            Cursor = LibContent.CursorIBeam;
            DisabledKeys = new Keys[] { };
            HideSelection = true;
            HoverColor = Color.GhostWhite * .7f;
            InputHoldingTime = 30;
            MaxLength = 0;
            Multiline = false;
            PasswordChar = '*';
            pointerLine = 0;
            pointerLocale = 0;
            PointerSpeed = 15;
            ReadOnly = false;
            SelectedColor = Color.LightGray;
            SelectionColor = Color.Blue;
            UseSystemPassword = false;
            #endregion

            #region Events
            Paint += eventPaint;
            #endregion
        }

        public override void Update()
        {
            base.Update();

            if (Selected && !ReadOnly)
                foreach (Keys key in Input.PressedKeys)
                    // If not special and (not disabled or not in disabled)
                    if (Array.IndexOf(keyboardSpecialKeys, key) == -1 && Array.IndexOf(DisabledKeys, key) == -1 && (AvailableKeys.Length > 0 ? Array.IndexOf(AvailableKeys, key) >= 0 : true))
                    {
                        bool keyExists = holdingKeys.ContainsKey(key);

                        if (keyExists && holdingKeys[key] > 0)
                            holdingKeys[key]--;
                        else
                        {
                            handleKeyPressed(key);
                            if (!keyExists)
                                // Add timer
                                holdingKeys.Add(key, InputHoldingTime);
                        }
                        break;
                    }

            // Stopped holding = remove timer
            Keys[] keyList = holdingKeysList;
            for (int i = 0; i < keyList.Length; i++)
                if (!Input.HoldingKey(keyList[i]))
                    holdingKeys.Remove(keyList[i]);

            // Pointer
            if (Selected && --pointerSpeed <= 0)
            {
                pointerSpeed = defaultPointerSpeed;

                string textSlice = Text.Substring(0, Math.Min(pointerLocale, Text.Length)) + (pointerLocale == 0 ? " " : "  ");
                float sliceSize = Math.Max(1, (int)Font.SpriteFont.MeasureString(textSlice).X); // >= 0

                float spaceSize = (int)Font.SpriteFont.MeasureString(" ").X;
                string spaces = (" ").Repeat((int)(sliceSize / spaceSize));

                pointerDisplay.Text = spaces + (defaultPointerSpeed == 0 ? "|" : (pointerDisplay.Text.Contains("|") ? "" : "|"));
            }
        }

        #region Methods
        private int movePointer(int p)
        {
            int oldPointerLocale = PointerLocale;
            PointerLocale += p;

            if (oldPointerLocale == PointerLocale)
                PointerLine += p;

            return PointerLocale;
        }

        private void handleKeyPressed(Keys key)
        {
            bool shift = false;
            if (Array.IndexOf(DisabledKeys, Keys.LeftShift) == -1)
                shift = Input.HoldingKey(Keys.LeftShift);
            if (!shift && Array.IndexOf(DisabledKeys, Keys.RightShift) == -1)
                shift = Input.HoldingKey(Keys.RightShift);

            // End selection
            if (key == Keys.Enter)
            {
                if (shift)
                {
                    if (Multiline)
                        Text += "\n";
                }
                else
                    Selected = false;
                return;
            }
            // Remove
            else
            {
                int len = Text.Length;

                if (key == Keys.Back)
                {
                    if (len > 0)
                    {
                        Text = Text.Remove(PointerLocale, 1);

                        movePointer(-1);
                    }

                    return;
                } 
                // Delete
                else if (key == Keys.Delete || (shift && key == Keys.Decimal))
                {
                    if (len > 0 && (PointerLocale == 0 || PointerLocale < len - 1))
                    {
                        Text = Text.Remove(PointerLocale + (PointerLocale == 0 ? 0 : 1), 1);
                    }
                    return;
                }
            }
            // Goes to the end
            if (key == Keys.End)
            {
                // End
                return;
            }
            // Maximum
            else if (MaxLength >= 1 && Text.Length >= MaxLength) return;
            // Horizontal arrows
            else if (key == Keys.Left)
            {
                int oldPointer = PointerLocale;
                movePointer(-1);

                if (shift)
                {
                    if (oldPointer != PointerLocale)
                    {
                        if (selectedText.DisplayedText.Length == 0)
                        {
                            selectedText.BackColor = SelectionColor;
                            selectedText.Tag = "left";
                        }
                        int dir = (string)selectedText.Tag == "left" ? 1 : -1;

                        string newText = Text[PointerLocale].ToString();
                        // TODO : Needs to change the location because it is left to right
                        selectedText.Size = new Vector2(selectedText.Size.X + (Font.SpriteFont.MeasureString(newText).X * dir), Size.Y);

                        if (dir > 0)
                            selectedText.DisplayedText = newText + selectedText.DisplayedText;
                        else
                            selectedText.DisplayedText.Remove(1, 1);
                    }
                }
                else
                    if (selectedText.DisplayedText.Length == 0)
                    {
                        selectedText.BackColor = Color.Transparent;
                        selectedText.Tag = null;
                    }

                return;
            }
            else if (key == Keys.Right)
            {
                int oldPointer = PointerLocale;
                movePointer(1);

                if (shift)
                {
                    if (oldPointer != PointerLocale)
                    {
                        // Start selection
                        if (selectedText.DisplayedText.Length == 0)
                        {
                            selectedText.BackColor = SelectionColor;
                            selectedText.Tag = "right";
                        }
                        int dir = (string)selectedText.Tag == "right" ? 1 : -1;

                        string newText = Text[PointerLocale].ToString();
                        selectedText.Size = new Vector2(selectedText.Size.X + (Font.SpriteFont.MeasureString(newText).X * dir), Size.Y);

                        if (dir > 0)
                            selectedText.DisplayedText += newText;
                        else
                            selectedText.DisplayedText.Remove(selectedText.DisplayedText.Length - 1, 1);
                    }
                }
                else
                    if (selectedText.DisplayedText.Length == 0)
                    {
                        selectedText.BackColor = Color.Transparent;
                        selectedText.Tag = null;
                    }

                return;
            }
            // Vertical arrows
            else if (key == Keys.Up)
            {
                PointerLine--;
                return;
            }
            else if (key == Keys.Down)
            {
                PointerLine++;
                return;
            }

            bool ctrl = false;
            if (Array.IndexOf(DisabledKeys, Keys.LeftControl) == -1)
                ctrl = Input.HoldingKey(Keys.LeftControl);
            if (!ctrl && Array.IndexOf(DisabledKeys, Keys.RightControl) == -1)
                ctrl = Input.HoldingKey(Keys.RightControl);

            if (ctrl)
            {
                if (Input.KeyDown(Keys.V)) // CTRL + V
                {
                    Text += System.Windows.Forms.Clipboard.GetText();
                    return;
                }
                else if (Input.KeyDown(Keys.Z)) // CTRL + Z
                {
                    int index = Math.Max(history.Count - 2, 0);
                    Text = (history.Count > 1 ? history[index] : string.Empty);
                    if (history.Count > 0)
                    {
                        redoHistory.Add(history[index]);
                        history.RemoveAt(index);
                    }
                    return;
                }
                else if (Input.KeyDown(Keys.Y)) // CTRL + Y
                {
                    // Undo e Redo functions are bad coded, indeed.
                    if (redoHistory.Count > 0)
                    {
                        int index = Math.Max(redoHistory.Count - 2, 0);
                        Text = redoHistory[index];
                        redoHistory.RemoveAt(index);
                    }
                    return;
                }
            }

            bool ctrlAlt = false;
            if (ctrl)
            {
                if (Array.IndexOf(DisabledKeys, Keys.LeftAlt) == -1)
                    ctrlAlt = Input.HoldingKey(Keys.LeftAlt);
                if (!ctrlAlt)
                    if (Array.IndexOf(DisabledKeys, Keys.RightAlt) == -1)
                        ctrlAlt = Input.HoldingKey(Keys.RightAlt);
                if (!ctrlAlt) return;
            }

            bool capsLock = Array.IndexOf(DisabledKeys, Keys.CapsLock) == -1 && Console.CapsLock;

            // Letters
            if (key >= Keys.A && key <= Keys.Z && !ctrlAlt)
            {
                string keyValue = key.ToString();
                insert((shift ? (capsLock ? keyValue.ToLower() : keyValue) : (capsLock ? keyValue : keyValue.ToLower())));
            }
            // Numpad numbers
            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9 && !ctrlAlt)
                insert(((int)key - 96).ToString());
            // Keyboard numbers
            else if (key >= Keys.D0 && key <= Keys.D9)
            {
                int num = (int)key - 48;

                if (shift)
                    insert(keyboardNumberShift[num]);
                else if (ctrlAlt)
                    if (keyboardNumberCtrlAlt[num] != string.Empty)
                        insert(keyboardNumberCtrlAlt[num]);
                    else
                        return;
                else
                    insert(num.ToString());
            }
            // Special characters
            else
            {
                if (key == Keys.Add) // + on numpad
                    insert("+");
                else if (key == Keys.Decimal) // , on numpad
                    insert("."); // Handling SHIFT in handleKeyboardAction
                else if (key == Keys.Divide) // / on numpad
                    insert("/");
                else if (key == Keys.Multiply) // * on numpad
                    insert("*");
                else if (key == Keys.OemBackslash) // \
                    insert((shift ? "|" : "\\"));
                else if (key == Keys.OemCloseBrackets) // ]
                    insert((shift ? "}" : (ctrlAlt ? "º" : "]")));
                else if (key == Keys.OemComma) // ,
                    insert((shift ? "<" : ","));
                else if (key == Keys.OemMinus) // -
                    insert((shift ? "_" : "-"));
                else if (key == Keys.OemOpenBrackets) // ]
                    insert((shift ? "{" : (ctrlAlt ? "ª" : "[")));
                else if (key == Keys.OemPeriod) // .
                    insert((shift ? ">" : "."));
                else if (key == Keys.OemPlus) // =
                    insert((shift ? "+" : (ctrlAlt ? "§" : "=")));
                else if (key == Keys.OemQuestion) // /
                    insert((shift ? "?" : (ctrlAlt ? "°" : "/")));
                else if (key == Keys.OemQuotes) // '
                    insert((shift ? "\"" : "'"));
                else if (key == Keys.OemSemicolon) // ;
                    insert((shift ? ":" : ";"));
                else if (key == Keys.OemTilde) // ~
                    insert((shift ? "^" : "~"));
                else if (key == Keys.Space) //  
                    insert(" ");
                else if (key == Keys.Subtract) // - on numpad
                    insert("-");
                else if (key == Keys.Tab && AcceptsTab) //   
                    insert("\t"); // Doesn't seem to be working
            }
        }

        private void insert(string value)
        {
            // Adds the character in the given position of the pointer
            Text = Text.Insert(Text.Length == 0 ? 0 : (movePointer(1) + 1), value);
        }

        private int lineLength()
        {
            int ret = 0;

            string[] lines = Lines;
            for (int i = 0; i < PointerLine - 1; i++)
                ret += lines[i].Length;

            return ret;
        }

        private void textToPassword()
        {
            if (UseSystemPassword)
                label.DisplayedText = PasswordChar.ToString().Repeat(label.DisplayedText.Length);
        }
        #endregion

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (selectedText != null)
                selectedText.Draw(e.SpriteBatch);

            if (label != null)
                label.Draw(e.SpriteBatch);

            if (pointerDisplay != null)
                pointerDisplay.Draw(e.SpriteBatch);
        }
        #endregion
    }
}
