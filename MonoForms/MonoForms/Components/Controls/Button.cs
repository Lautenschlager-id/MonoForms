using Microsoft.Xna.Framework;

namespace MonoForms
{
    class Button : Control
    {
        #region Property
        public override float Alpha
        {
            set
            {
                base.Alpha = value;

                if (label != null)
                    label.Alpha = value;
            }
        }

        public override bool AutoEllipsis
        {
            set
            {
                base.AutoEllipsis = value;

                if (label != null)
                    label.AutoEllipsis = value;
            }
        }

        public override bool AutoSize
        {
            set
            {
                base.AutoSize = value;

                if (label != null)
                    label.AutoSize = value;
            }
        }

        public override bool Enabled
        {
            set
            {
                base.Enabled = value;

                if (label != null)
                    label.Enabled = value;
            }
        }

        public override MonoSpriteFont Font
        {
            get => label.Font;
            set => label.Font = value;
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

                if (label != null)
                    label.Location  = value;
            }
        }

        public override Vector2 Size
        {
            set
            {
                base.Size = value;

                if (label != null)
                    label.Size = value;
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
            }
        }
        #endregion

        #region ControlProperty
        
        #endregion

        #region ControlPrivateProperty
        private Label label;
        #endregion

        public Button()
        {
            #region Control
            label = new Label()
            {
                AutoEllipsis = true
            };
            #endregion

            #region Property
            BackColor = Color.GhostWhite * .6f;
            Cursor = LibContent.CursorLink;
            Size = new Vector2(110, 30);
            #endregion

            #region Events
            Paint += eventPaint;
            #endregion
        }

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (label != null)
                label.Draw(e.SpriteBatch);
        }
        #endregion
    }
}
