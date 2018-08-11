using Microsoft.Xna.Framework;
using System;

namespace MonoForms
{
    class CheckBox : Control
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
                base.Location = new Vector2(value.X, value.Y + 1);

                if (stateLabel != null)
                    stateLabel.Location = value;

                if (contentLabel != null)
                {
                    value.X += Size.X + 5;
                    contentLabel.Location = value;
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
        public bool IsChecked
        {
            get => isChecked;
        }
        #endregion

        #region ControlPrivateProperty
        private Label contentLabel, stateLabel;

        private bool isChecked = false;
        #endregion

        #region Event
        public event EventHandler<MonoValueEventArg> CheckedChanged;
        #endregion

        public CheckBox()
        {
            #region Control
            contentLabel = new Label()
            {
                AutoSize = true
            };

            stateLabel = new Label()
            {
                Font = Game.SystemFont,
                AutoSize = true,
            };
            #endregion

            #region Properties
            BackColor = Color.GhostWhite * .6f;
            SetSize(12);
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

            // Adjusts the contentLabel location
            Location = Location;
        }

        private void ToggleChecked(bool? value = null)
        {
            stateLabel.Text = (isChecked = (value ?? !isChecked)) ? "✔" : string.Empty;
        }
        #endregion

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (contentLabel != null)
                contentLabel.Draw(e.SpriteBatch);

            if (stateLabel != null)
                stateLabel.Draw(e.SpriteBatch);
        }
        #endregion
    }
}
