using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoForms
{
    class ListBox : Control
    {
        // Check if properties triggers in the buttons
        #region Property
        public override float Alpha
        {
            set
            {
                base.Alpha = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Alpha = value;
            }
        }

        public override bool Enabled
        {
            set
            {
                base.Enabled = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Enabled = value;
            }
        }

        public override MonoSpriteFont Font
        {
            set
            {
                base.Font = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Font = value;
            }
        }

        public override Vector2 Location
        {
            set
            {
                base.Location = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Location = new Vector2(Location.X, Location.Y + (20 * i));
            }
        }

        public override Vector2 Size
        {
            set
            {
                base.Size = value;

                for (int i = 0; i < items.Count; i++)
                    items[i].Size = new Vector2(value.X, items[i].Size.Y);

                // Normalizes the size
                Location = Location;
            }
        }

        public override bool Visible
        {
            set
            {
                base.Visible = value;

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

        public List<string> Items { get; private set; }

        public int MaxLength { get; set; }

        public int SelectedIndex { get; private set; }

        public Color SelectionColor { get; set; }

        public string SelectedText { get; private set; }

        public bool Sorted { get; private set; }
        #endregion

        #region ControlPrivateProperty
        private Color itemBackColor;

        private Color itemForeColor;

        private List<Button> items = new List<Button>();

        private Button selectedItem;
        #endregion

        #region ControlEvent
        public event EventHandler<MonoItemEventArg> ItemAdded;

        public event EventHandler<MonoItemEventArg> ItemRemoved;

        public event EventHandler<MonoIndexEventArg> SelectedIndexChanged;
        #endregion

        public ListBox(bool sorted = false)
        {
            #region Properties
            itemBackColor = Color.GhostWhite * .5f;
            itemForeColor = Color.Black;
            Items = new List<string>(); // Use AddItem otherwise MaxLength will never work.
            MaxLength = -1;
            SelectedIndex = -1;
            SelectionColor = Color.Blue * .5f;
            Size = new Vector2(130, 0);
            Sorted = sorted;
            #endregion

            #region Events
            Paint += eventPaint;
            #endregion
        }

        public override void Update()
        {
            base.Update();

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
                    Text = item,
                    AutoSize = true,
                    Size = new Vector2(Size.X, 20),
                    Tag = items.Count,
                    Location = new Vector2(Location.X, Location.Y + (20 * items.Count))
                };
                newItem.MouseUp += eventSelect;

                items.Add(newItem);

                base.Size = new Vector2(Size.X, Size.Y + 20);

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
                items[i].Location = new Vector2(Location.X, Location.Y + (20 * (int)(items[i].Tag = i)));
        }
        #endregion

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            for (int i = 0; i < items.Count; i++)
                items[i].Draw(e.SpriteBatch);
        }

        private void eventSelect(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (SelectedIndex == (int)button.Tag)
                return;
            else
                SelectedIndex = (int)button.Tag;
            SelectedText = button.Text;

            if (selectedItem != null)
                selectedItem.BackColor = ItemBackColor;

            selectedItem = button;
            button.BackColor = SelectionColor;

            SelectedIndexChanged?.Invoke(this, new MonoIndexEventArg(SelectedIndex, SelectedText));
        }
        #endregion
    }
}