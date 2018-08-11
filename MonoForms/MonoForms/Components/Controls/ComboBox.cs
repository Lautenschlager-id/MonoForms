using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoForms
{
    class ComboBox : Control
    {
        // Check if properties triggers in the buttons
        #region Property
        public override float Alpha
        {
            set
            {
                base.Alpha = value;

                if (label != null)
                    label.Alpha = value;

                if (symbol != null)
                    symbol.Alpha = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Alpha = value;
            }
        }

        public override bool Enabled
        {
            set
            {
                base.Enabled = value;

                if (label != null)
                    label.Enabled = value;

                if (symbol != null)
                    symbol.Enabled = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Enabled = value;
            }
        }

        public override MonoSpriteFont Font
        {
            set
            {
                base.Font = value;

                if (label != null)
                    label.Font = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Font = value;
            }
        }

        public override Color ForeColor
        {
            set
            {
                base.ForeColor = value;

                if (label != null)
                    label.ForeColor = value;

                if (symbol != null)
                    symbol.ForeColor = value;
            }
        }

        public override Vector2 Location
        {
            set
            {
                base.Location = value;

                if (label != null)
                    label.Location = value;

                // TODO: rewrite with TextPosition
                if (symbol != null)
                {
                    float X = value.X + Size.X - symbol.Font.SpriteFont.MeasureString(symbol.Text).X;
                    symbol.Location = new Vector2(X, value.Y);
                }

                 for (int i = 0; i < items.Count; i++)
                    items[i].Location = new Vector2(Location.X, Location.Y + Size.Y + (20 * i));
            }
        }

        public override Vector2 Size
        {
            set
            {
                base.Size = value;

                if (label != null)
                    label.Size = new Vector2(value.X * .9f, value.Y * .9f); // Size tolerance

                for (int i = 0; i < items.Count; i++)
                    items[i].Size = new Vector2(value.X, items[i].Size.Y);

                // Normalizes the size
                Location = Location;
            }
        }

        public override string Text
        {
            get => label.Text;
            set => label.Text = value;
        }

        public override bool Visible
        {
            set
            {
                base.Visible = value;

                if (label != null)
                    label.Visible = value;

                if (symbol != null)
                    symbol.Visible = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Visible = value;
            }
        }
        #endregion

        #region ControlProperty
        // TODO: Format property for MaskedTextBox?
        public Color ItemBackColor
        {
            get => itemBackColor;
            set
            {
                itemBackColor = value;
                for (int i = 0; i < items.Count; i++)
                    items[i].BackColor = itemBackColor;
            }
        }
        
        public Color ItemForeColor
        {
            get => itemForeColor;
            set
            {
                itemForeColor = value;
                for (int i = 0; i < items.Count; i++)
                    items[i].ForeColor = itemForeColor;
            }
        }

        public Color ItemHoverColor
        {
            get => itemHoverColor;
            set
            {
                itemHoverColor = value;
                for (int i = 0; i < items.Count; i++)
                    items[i].HoverColor = itemHoverColor;
            }
        }

        public List<string> Items { get; private set; }

        public int MaxLength { get; set; }

        public int SelectedIndex { get; private set; }

        public string SelectedText { get; private set; }

        public bool Sorted { get; private set; }
        #endregion

        #region ControlPrivateProperty
        private bool isOpen = false;

        private Color itemBackColor;

        private Color itemForeColor;

        private Color itemHoverColor;

        private List<Button> items = new List<Button>();

        private Label label, symbol;
        #endregion

        #region ControlEvent
        public event EventHandler<MonoItemEventArg> ItemAdded;

        public event EventHandler<MonoItemEventArg> ItemRemoved;

        public event EventHandler<MonoIndexEventArg> SelectedIndexChanged;
        #endregion

        public ComboBox(bool sorted = false)
        {
            #region Control
            // TextAlign = Left
            label = new Label()
            {
                AutoEllipsis = true
            };

            // TextAlign = Right
            symbol = new Label()
            {
                Font = Game.SystemFont,
                Text = "▼",
                AutoEllipsis = true
            };
            #endregion

            #region Properties
            BackColor = Color.GhostWhite * .8f;
            itemBackColor = Color.GhostWhite * .5f;
            itemForeColor = Color.Black;
            itemHoverColor = Color.Aquamarine;
            Items = new List<string>(); // Use AddItem otherwise MaxLength will never work.
            MaxLength = -1;
            SelectedIndex = -1;
            Size = new Vector2(130, 25);
            Sorted = sorted;
            Text = "Select";
            #endregion

            #region Events
            MouseDown += eventMouseDown;
            Paint += eventPaint;
            #endregion
        }

        public override void Update()
        {
            base.Update();
            if (isOpen)
                for (int i = 0; i < items.Count; i++)
                    items[i].Update();
        }

        #region Method
        public void AddItem(string item)
        {
            if (MaxLength < 0 || Items.Count < MaxLength)
            {
                Items.Add(item);

                Button newItem = new Button()
                {
                    BackColor = ItemBackColor,
                    Cursor = LibContent.CursorArrow,
                    ForeColor = ItemForeColor,
                    HoverColor = ItemHoverColor,
                    Text = item,
                    AutoSize = true,
                    Size = new Vector2(Size.X, 20),
                    Tag = items.Count,
                    Location = new Vector2(Location.X, Location.Y + Size.Y + (20 * items.Count))
                };
                newItem.MouseUp += eventSelect;

                items.Add(newItem);

                if (Sorted)
                    OrderItems();

                ItemAdded?.Invoke(this, new MonoItemEventArg(newItem));
            }
        }

        public void RemoveItem(Button item)
        {
            RemoveItem((int)item.Tag);
        }

        public void RemoveItem(int id)
        {
            object button = null;

            if (items[id] != null)
            {
                button = items[id];
                items.RemoveAt(id);
            }
            if (Items[id] != null)
                Items.RemoveAt(id);

            if (button != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].Tag = i;
                    items[i].Location = new Vector2(Location.X, Location.Y + Size.Y + (21 * i));
                }

                ItemRemoved?.Invoke(this, new MonoItemEventArg(button));
            }
        }

        public void RemoveItem(string item)
        {
            Button button = items.Where(i => i.Text == item) as Button;
            RemoveItem((int)button.Tag);
        }

        private void OrderItems()
        {
            items = items.OrderBy(b => b.Text).ToList();

            for (int i = 0; i < items.Count; i++)
                items[i].Location = new Vector2(Location.X, Location.Y + Size.Y + (20 * (int)(items[i].Tag = i)));
        }
        #endregion

        #region ControlEvent
        private void eventMouseDown(object sender, MonoMouseEventArg e)
        {
            if (e.RightButton) return;

            symbol.Text = ((isOpen = !isOpen) ? "▲" : "▼");
        }

        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (label != null)
                label.Draw(e.SpriteBatch);
            if (symbol != null)
                symbol.Draw(e.SpriteBatch);

            if (isOpen)
                for (int i = 0; i < items.Count; i++)
                    items[i].Draw(e.SpriteBatch);
        }

        private void eventSelect(object sender, EventArgs e)
        {
            symbol.Text = ((isOpen = !isOpen) ? "▲" : "▼");

            Button button = sender as Button;

            if (SelectedIndex == (int)button.Tag)
                return;
            else
                SelectedIndex = (int)button.Tag;
            SelectedText = button.Text;

            Text = button.Text;

            SelectedIndexChanged?.Invoke(this, new MonoIndexEventArg(SelectedIndex, SelectedText));
        }
        #endregion
    }
}