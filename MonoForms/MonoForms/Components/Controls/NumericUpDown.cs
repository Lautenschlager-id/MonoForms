using Microsoft.Xna.Framework;
using System;

namespace MonoForms
{
    class NumericUpDown : Control
    {
        #region Property
        public override float Alpha
        {
            set
            {
                base.Alpha = value;

                if (label != null)
                    label.Alpha = value;

                if (up != null && down != null)
                    down.Alpha = up.Alpha = MathHelper.Min(value, .3f) - .3f;
            }
        }

        public override bool Enabled
        {
            set
            {
                base.Enabled = value;

                if (label != null)
                    label.Enabled = value;

                if (up != null && down != null)
                    down.Enabled = up.Enabled = value;
            }
        }

        public override MonoSpriteFont Font
        {
            set
            {
                base.Font = value;

                if (label != null)
                    label.Font = value;
            }
        }

        public override Color ForeColor
        {
            set
            {
                base.ForeColor = value;

                if (label != null)
                    label.ForeColor = value;
            }
        }

        public override Vector2 Location
        {
            set
            {
                base.Location = value;

                label.Location = value;

                if (up != null && down != null)
                {
                    float X = value.X + Size.X - up.Size.X; // up/down = same size

                    up.Location = new Vector2(X, value.Y - .5f);
                    down.Location = new Vector2(X, value.Y + up.Size.Y);
                }
            }
        }

        public override Vector2 Size
        {
            set
            {
                base.Size = value;

                if (label != null)
                    label.Size = new Vector2(value.X - up.Size.X, value.Y);

                if (up != null && down != null)
                    down.Size = up.Size = new Vector2(15, value.Y / 2);

                // Normalizes the size
                Location = Location;
            }
        }

        public override bool Visible
        {
            set
            {
                base.Visible = value;

                if (label != null)
                    label.Visible = value;

                if (up != null && down != null)
                    down.Visible = up.Visible = value;
            }
        }
        #endregion

        #region ControlProperty
        public int DecimalPlaces
        {
            get => decimalPlaces;
            set => MathHelper.Clamp(value, 0, 99);
        }

        public decimal Increment
        {
            get => increment;
            set
            {
                if (value <= 0) value = 1;
                increment = value;
            }
        }

        public bool InterceptArrowKeys { get; set; } // TODO (use up and down to change value)

        public decimal Maximum { get; set; }

        public decimal Minimum { get; set; }

        public bool ThousandsSeparator { get; set; }

        public bool UpDownAlign { get; set; } // TODO

        public Color UpDownForeColor
        {
            get => down.ForeColor;
            set
            {
                down.ForeColor = up.ForeColor = value;
            }
        }

        public decimal Value
        {
            get => value;
            set
            {
                this.value = value.Clamp(Minimum, Maximum);

                if (label != null)
                    label.Text = Value.ToString((ThousandsSeparator ? "N" : string.Empty) + DecimalPlaces);

                ValueChanged?.Invoke(this, new MonoValueEventArg(Value));
            }
        }
        #endregion

        #region ControlPrivateProperty
        private bool clickTimeAfterDelay
        {
            get => (lastClickTime > DateTime.MinValue && (DateTime.Now - lastClickTime).TotalSeconds > .8f);
        }

        private int decimalPlaces;

        private Button down;

        private decimal increment;

        private Label label;

        private DateTime lastClickTime = DateTime.MinValue;

        private Button up;

        private decimal value;
        #endregion

        #region ControlEvent
        public event EventHandler<MonoValueEventArg> ValueChanged;
        #endregion

        public NumericUpDown()
        {
            #region Control
            down = new Button()
            {
                Alpha = .7f,
                BackColor = Color.Transparent,
                Cursor = LibContent.CursorArrow,
                Font = Game.SystemFont,
                Text = "▼",
                Size = new Vector2(15, 10)
            };
            label = new Label()
            {
                Text = "0"
            };
            up = new Button()
            {
                Alpha = .7f,
                BackColor = Color.Transparent,
                Cursor = LibContent.CursorArrow,
                Font = Game.SystemFont,
                Text = "▲",
                Size = new Vector2(15, 10)
            };
            #endregion

            #region Properties
            BackColor = Color.GhostWhite * .8f;
            DecimalPlaces = 0;
            Increment = 1;
            Maximum = 100;
            Minimum = 0;
            Size = new Vector2(130, 25);
            ThousandsSeparator = false;
            UpDownAlign = false;
            Value = 0;
            #endregion

            #region Events
            down.MouseDown += Sub;
            down.MouseLeave += Release;
            down.MousePressing += SubPressing;
            Paint += eventPaint;
            up.MouseDown += Add;
            up.MouseLeave += Release;
            up.MousePressing += AddPressing;
            #endregion
        }

        public override void Update()
        {
            base.Update();

            down.Update();
            up.Update();
        }

        #region Method
        private void Add(object sender, MonoMouseEventArg e)
        {
            Value += Increment;
            lastClickTime = DateTime.Now;
        }

        private void AddPressing(object sender, MonoMouseEventArg e)
        {
            if (clickTimeAfterDelay)
                Value += Increment;
        }

        private void Sub(object sender, MonoMouseEventArg e)
        {
            Value -= Increment;
            lastClickTime = DateTime.Now;
        }

        private void SubPressing(object sender, MonoMouseEventArg e)
        {
            if (clickTimeAfterDelay)
                Value -= Increment;
        }

        private void Release(object sender, EventArgs e)
        {
            lastClickTime = DateTime.MinValue;
        }
        #endregion

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (label != null)
                label.Draw(e.SpriteBatch);

            if (down != null)
                down.Draw(e.SpriteBatch);

            if (up != null)
                up.Draw(e.SpriteBatch);
        }
        #endregion
    }
}
