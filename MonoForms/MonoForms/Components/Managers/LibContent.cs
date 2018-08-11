using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoForms
{
    static class LibContent
    {
        #region Control
        public static Texture2D ControlCircle { get; private set; }
        #endregion

        #region Cursor
        public static Texture2D CursorAppStarting { get; private set; }
        public static Texture2D CursorArrow { get; private set; }
        public static Texture2D CursorHelp { get; private set; }
        public static Texture2D CursorIBeam { get; private set; }
        public static Texture2D CursorLink { get; private set; }
        public static Texture2D CursorUpArrow { get; private set; }
        public static Texture2D CursorWait { get; private set; }
        #endregion

        #region Sample
        public static Texture2D Image { get; private set; }
        #endregion

        public static void LoadContent(ContentManager content)
        {
            #region Control
            ControlCircle = content.Load<Texture2D>("Controls/circle");
            #endregion

            #region Cursors
            CursorAppStarting = content.Load<Texture2D>("Cursors/appstarting");
            CursorArrow = content.Load<Texture2D>("Cursors/arrow");
            CursorHelp = content.Load<Texture2D>("Cursors/help");
            CursorIBeam = content.Load<Texture2D>("Cursors/ibeam");
            CursorLink = content.Load<Texture2D>("Cursors/link");
            CursorUpArrow = content.Load<Texture2D>("Cursors/uparrow");
            CursorWait = content.Load<Texture2D>("Cursors/wait");
            #endregion

            #region Samples
            Image = content.Load<Texture2D>("Samples/sample_image");
            #endregion
        }
    }
}
