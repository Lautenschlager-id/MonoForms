using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoForms
{
    class PictureBox : Control
    {
        #region Property
        public override bool AutoSize
        {
            set
            {
                if (base.AutoSize = value && Image != null)
                    Size = Image.Size();
            }
        }

        public Texture2D Image { get; set; }
        #endregion

        public PictureBox()
        {
            #region Properties
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Size = new Vector2(100, 100);
            #endregion

            #region Events
            Paint += eventPaint;
            #endregion
        }

        #region ControlEvent
        private void eventPaint(object sender, MonoPaintEventArg e)
        {
            if (Image != null)
                e.SpriteBatch.Draw(Image, ControlShape, ControlShape, ForeColor * Alpha);
        }
        #endregion
    }
}
