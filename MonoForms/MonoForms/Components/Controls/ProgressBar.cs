using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoForms
{
    class ProgressBar : Control
    {
        #region Property
        public override Vector2 Location
        {
            set
            {
                base.Location = value;

                if (loadBar != null)
                {
                    loadBar.X = (int)value.X + 5;
                    loadBar.Y = (int)value.Y + 5;
                }
            }
        }

        public override Vector2 Size
        {
            set
            {
                base.Size = Vector2.Max(value, new Vector2(1, 6));

                if (loadBar != null)
                {
                    //PerformStep(true);
                    loadBar.Height = (int)Size.Y - 10;
                }
            }
        }
        #endregion

        #region ControlProperty
        public int Maximum { get; set; }

        public int Minimum { get; set; }

        public int Step { get; set; }

        public int Value
        {
            get => value;
            set => this.value = value.Clamp(Minimum, Maximum);
        }
        #endregion

        #region ControlPrivateProperty
        private Rectangle loadBar;

        private Texture2D texture;

        private int value;
        #endregion

        #region ControlEvent
        public event EventHandler<MonoValueEventArg> ValueChanged;
        #endregion

        public ProgressBar()
        {
            #region Properties
            BackColor = Color.GhostWhite * .7f;
            ForeColor = Color.ForestGreen;
            Maximum = 100;
            Minimum = 0;
            Size = new Vector2(300, 30);
            Step = 5;
            value = 0;
            #endregion

            #region Control
            loadBar = new Rectangle()
            {
                Width = 1,
                Height = (int)Size.Y - 10
            };

            texture = new Texture2D(Game.Instance.GraphicsDevice, 1, 1);
            texture.Fill();
            #endregion

            #region Events
            Paint += eventPaint;
            #endregion
        }

        #region Method
        public void PerformStep()
        {
            Value += Step;

            loadBar.Width = (int)(MathHelper.Max(1, Value * (Size.X / Maximum) - 10));

            ValueChanged?.Invoke(this, new MonoValueEventArg(Value));
        }
        #endregion

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (loadBar != null)
                e.SpriteBatch.Draw(texture, loadBar, ForeColor * Alpha);
        }
        #endregion
    }
}
