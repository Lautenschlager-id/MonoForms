using Microsoft.Xna.Framework;
using System;

namespace MonoForms
{
    class LinkLabel : Label
    {
        #region ControlProperty
        public Color ActiveLinkColor { get; set; }

        public string Link
        {
            get => link;
            set
            {
                link = value;

                if (Text == null || Text == string.Empty)
                    Text = value;
            }
        }
        
        public Color LinkColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        public Color VisitedLinkColor { get; set; }
        #endregion

        #region ControlPrivateProperty
        private string link;
        #endregion

        public LinkLabel()
        {
            #region Properties
            ActiveLinkColor = Color.Red;
            autoSize = true;
            Cursor = LibContent.CursorLink;
            LinkColor = Color.Blue;
            VisitedLinkColor = Color.Purple;
            #endregion

            #region Events
            MousePressing += new EventHandler<MonoMouseEventArg>((sender, e) => { if (e.LeftButton) LinkColor = ActiveLinkColor; });
            MouseUp += eventMouseUp;
            #endregion
        }

        #region ControlEvent
        private void eventMouseUp(object sender, MonoMouseEventArg e)
        {
            if (e.RightButton) return;

            LinkColor = VisitedLinkColor;

            if (Link != null || Link != string.Empty)
                System.Diagnostics.Process.Start(Link);
        }
        #endregion
    }
}
