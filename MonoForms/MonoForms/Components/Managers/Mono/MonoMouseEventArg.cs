using Microsoft.Xna.Framework;

namespace MonoForms
{
    class MonoMouseEventArg : System.EventArgs
    {
        public bool LeftButton { get; private set; }

        public bool RightButton { get; private set; }

        public int ScrollValue { get; private set; }
        
        public MonoMouseEventArg(bool leftButton, bool rightButton)
        {
            LeftButton = leftButton;
            RightButton = rightButton;
        }

        public MonoMouseEventArg(int scrollValue)
        {
            ScrollValue = scrollValue;
        }
    }
}
