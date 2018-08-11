using Microsoft.Xna.Framework.Input;

namespace MonoForms
{
    class MonoKeyEventArg : System.EventArgs
    {
        public Keys Key { get; private set; }

        public MonoKeyEventArg(Keys key)
        {
            Key = key;
        }
    }
}
