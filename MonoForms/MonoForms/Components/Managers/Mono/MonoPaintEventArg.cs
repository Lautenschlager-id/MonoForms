using Microsoft.Xna.Framework.Graphics;

namespace MonoForms
{
    class MonoPaintEventArg : System.EventArgs
    {
        public SpriteBatch SpriteBatch { get; private set; }

        public MonoPaintEventArg(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }
    }
}
