using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoForms
{
    class RadioButton : Control
    {
        #region Property
        public override float Alpha
        {
            set
            {
                base.Alpha = value;

                if (contentLabel != null)
                    contentLabel.Alpha = value;

                if (stateLabel != null)
                    stateLabel.Alpha = value;
            }
        }

        public override Color BackColor
        {
            set { }
        }

        public override bool Enabled
        {
            set
            {
                base.Enabled = value;

                if (contentLabel != null)
                    contentLabel.Enabled = value;

                if (stateLabel != null)
                    stateLabel.Enabled = value;
            }
        }

        public override MonoSpriteFont Font
        {
            get => contentLabel.Font;
            set => contentLabel.Font = value;
        }

        public override Color ForeColor
        {
            get => contentLabel.ForeColor;
            set
            {
                if (contentLabel != null)
                    contentLabel.ForeColor = value;
            }
        }

        public override Vector2 Location
        {
            set
            {
                base.Location = new Vector2(value.X, value.Y + 2);

                if (stateLabel != null)
                {
                    int X = (int)(value.X + 2);
                    int Y = (int)(value.Y - 1);
                    stateLabel.Location = new Vector2(X, Y);
                }

                if (contentLabel != null)
                {
                    int X = (int)(value.X + Size.X + 5);
                    contentLabel.Location = new Vector2(X, value.Y);
                }
            }
        }

        public override Vector2 Size
        {
            set { }
        }

        public override string Text
        {
            get => contentLabel.Text;
            set => contentLabel.Text = value;
        }

        public override bool Visible
        {
            set
            {
                base.Visible = value;

                if (contentLabel != null)
                    contentLabel.Visible = value;

                if (stateLabel != null)
                    stateLabel.Visible = value;
            }
        }
        #endregion

        #region ControlProperty
        public Color BgColor { get; set; }

        public bool IsChecked
        {
            get => isChecked;
        }
        #endregion

        #region ControlPrivateProperty
        private Texture2D circle;
        
        private float circleSize;

        private Label contentLabel, stateLabel;

        private bool isChecked = false;
        #endregion

        #region Event
        public event EventHandler<MonoValueEventArg> CheckedChanged;
        #endregion

        public RadioButton()
        {
            #region Control
            contentLabel = new Label()
            {
                AutoSize = true
            };

            stateLabel = new Label()
            {
                Font = Game.SystemFont,
                AutoSize = true
            };
            #endregion

            #region Properties
            base.BackColor = Color.Transparent;
            BgColor = Color.GhostWhite * .8f;
            circle = LibContent.ControlCircle;
            SetSize(10);
            #endregion

            #region Events
            MouseUp += eventToggle;
            Paint += eventPaint;
            #endregion
        }

        #region Methods
        private void eventToggle(object sender, MonoMouseEventArg e)
        {
            if (e.RightButton) return;

            ToggleChecked();

            CheckedChanged?.Invoke(this, new MonoValueEventArg(isChecked));
        }

        public void SetSize(int size)
        {
            base.Size = new Vector2(size, size);

            circleSize = size / 100f;

            // Adjusts the contentLabel location
            Location = Location;
        }

        private void ToggleChecked(bool? value = null)
        {
            stateLabel.Text = (isChecked = (value ?? !isChecked)) ? "●" : string.Empty;
        }
        #endregion

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (circle != null)
                e.SpriteBatch.Draw(circle, Location, null, BgColor * Alpha, 0, Vector2.Zero, circleSize, SpriteEffects.None, 0);

            if (contentLabel != null)
                contentLabel.Draw(e.SpriteBatch);

            if (stateLabel != null)
                stateLabel.Draw(e.SpriteBatch);
        }
        #endregion
    }
}
