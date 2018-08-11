using Microsoft.Xna.Framework;

namespace MonoForms
{
    class MonoDimensionEventArg : System.EventArgs
    {
        public Vector2 CurrentDimension { get; private set; }

        public Vector2 OldDimension { get; private set; }
        
        public MonoDimensionEventArg(Vector2 oldDimension, Vector2 currentDimension)
        {
            OldDimension = oldDimension;
            CurrentDimension = currentDimension;
        }
    }
}
