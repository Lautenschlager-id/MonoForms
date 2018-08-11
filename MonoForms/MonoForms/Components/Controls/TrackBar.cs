using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoForms
{
    class TrackBar : Control
    {
        #region Property
        public override Vector2 Location
        {
            set
            {
                float x = value.X, y = value.Y;

                if (Orientation == Enums.TrackbarOrientation.Horizontal)
                    x = value.X - (Size.X / 2f);
                else
                    y = value.Y - (Size.Y / 2f);

                base.Location = new Vector2(x, y);

                if (line != null)
                {
                    line.X = (int)value.X;
                    line.Y = (int)value.Y;
                }
            }
        }
        #endregion

        #region ControlProperty
        public Texture2D LineBackgroundImage { get; set; }

        public int LineSize
        {
            get => (Orientation == Enums.TrackbarOrientation.Horizontal ? line.Width : line.Height);
            set
            {
                if (line != null)
                    if (Orientation == Enums.TrackbarOrientation.Horizontal)
                        line.Width = value;
                    else
                        line.Height = value;
            }
        }

        public int LineThickness
        {
            get => (Orientation == Enums.TrackbarOrientation.Horizontal ? line.Height : line.Width);
            set
            {
                if (line != null)
                    if (Orientation == Enums.TrackbarOrientation.Horizontal)
                        line.Height = value;
                    else
                        line.Width = value;
            }
        }

        public float Maximum { get; set; }
        
        public float Minimum { get; set; }

        public Enums.TrackbarOrientation Orientation { get; private set; }

        public float Value { get; set; }
        #endregion

        #region ControlPrivateProperty
        private Rectangle line;

        private Texture2D texture;
        #endregion

        #region ControlEvent
        public event EventHandler<MonoValueEventArg> ValueChanged;
        #endregion

        public TrackBar(Enums.TrackbarOrientation orientation = Enums.TrackbarOrientation.Horizontal)
        {
            #region Properties
            BackColor = Color.DimGray;
            ForeColor = Color.LightGray;
            HoverColor = Color.Gray;
            invertColors = true;
            Maximum = 100;
            Minimum = 0;
            Orientation = orientation;
            Size = new Vector2(20, 20);
            Value = 0;
            #endregion

            #region Control
            line = new Rectangle()
            {
                Width = (orientation == Enums.TrackbarOrientation.Horizontal ? (int)Maximum : 5),
                Height = (orientation == Enums.TrackbarOrientation.Vertical ? (int)Maximum : 5)
            };

            texture = new Texture2D(Game.Instance.GraphicsDevice, 1, 1);
            texture.Fill();
            #endregion

            #region Events
            MousePressing += eventPressing;
            // Not using the event Move because it would move the line too
            PaintBackground += eventPaintBg;
            #endregion
        }

        #region Method
        private void setHorizontalSliderLocation(int x)
        {
            x = x.Clamp(line.X, line.X + line.Width);

            base.Location = new Vector2(x - (int)(Size.X / 2), Location.Y);

            // Sets to (x-X), then normalize dividing by W and mul for MAX to get the actual value
            Value = (Maximum * (x - line.X) / line.Width).Clamp(Minimum, Maximum);
        }

        private void setVerticalSliderLocation(int y)
        {
            y = y.Clamp(line.Y, line.Y + line.Height);

            base.Location = new Vector2(Location.X, y - (int)(Size.Y / 2));

            // Sets to (y-Y), then normalize dividing by H and mul for MAX to get the actual value
            Value = (Maximum * (y - line.Y) / line.Height).Clamp(Minimum, Maximum);
        }
        #endregion

        #region ControlEvent
        private void eventPaintBg(object sender, MonoPaintEventArg e)
        {
            if (line != null)
            {
                if (LineBackgroundImage != null)
                    e.SpriteBatch.Draw(LineBackgroundImage, line, line, Color.White * Alpha);

                e.SpriteBatch.Draw(texture, line, BackColor * Alpha);
            }
        }

        private void eventPressing(object sender, MonoMouseEventArg e)
        {
            if (e.RightButton) return;

            if (Input.MouseMoved)
                if (Orientation == Enums.TrackbarOrientation.Horizontal)
                    setHorizontalSliderLocation((int)Input.MouseLocation.X);
                else
                    setVerticalSliderLocation((int)Input.MouseLocation.Y);

            ValueChanged?.Invoke(this, new MonoValueEventArg(Value));
        }
        #endregion
    }
}
