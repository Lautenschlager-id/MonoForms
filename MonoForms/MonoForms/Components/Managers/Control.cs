using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace MonoForms
{
    abstract class Control
    {
        #region Property
        public virtual float Alpha { get; set; }

        public virtual bool AutoEllipsis { get; set; }

        public virtual bool AutoSize
        {
            get => autoSize;
            set
            {
                if (autoSize = value)
                    Size = Font.SpriteFont.MeasureString(Text);
            }
        }

        public virtual Color BackColor { get; set; }

        public virtual Texture2D BackgroundImage { get; set; }

        public virtual Texture2D Cursor { get; set; }

        public virtual bool Enabled { get; set; }

        public virtual MonoSpriteFont Font
        {
            get => font;
            set
            {
                if (FontData.Collection.ContainsKey(value.Id))
                    font = FontData.Fonts.Find(f => f.Id == value.Id);
                else
                    font = FontData.Fonts.Find(f => f.Id == Game.DefaultFont.Id);
            }
        }

        public virtual Color ForeColor { get; set; }

        public virtual Color HoverColor { get; set; }

        public virtual Vector2 Location
        {
            get => location;
            set
            {
                controlShape.X = (int)value.X;
                controlShape.Y = (int)value.Y;

                Move?.Invoke(this, new MonoDimensionEventArg(location, value));

                location = value;
            }
        }

        public virtual Vector2 MaximumSize
        {
            get => maximumSize;
            set
            {
                maximumSize = value;

                // Triggers the Size_set_ to resize the control
                Size = Size;
            }
        }

        public virtual Vector2 MinimumSize
        {
            get => minimumSize;
            set
            {
                minimumSize = value;

                // Triggers the Size_set_ to resize the control
                Size = Size;
            }
        }

        public virtual string Name { get; set; }

        public virtual bool Selected { get; set; }

        public virtual Color SelectedColor { get; set; }

        public virtual Vector2 Size
        {
            get => size;
            set
            {
                value = Vector2.Clamp(value, MinimumSize, MaximumSize);

                controlShape.Width = (int)value.X;
                controlShape.Height = (int)value.Y;

                texture = new Texture2D(Game.Instance.GraphicsDevice, (int)value.X, (int)value.Y);
                texture.Fill();

                Resize?.Invoke(this, new MonoDimensionEventArg(new Vector2(size.X, size.Y), value));

                size = value;
            }
        }

        public virtual object Tag { get; set; }

        public virtual string Text { get; set; }

        public virtual bool Visible { get; set; }
        #endregion

        #region PrivateProperty
        protected bool autoSize;

        protected bool isHovering = false;

        protected MonoSpriteFont font;

        protected bool invertColors = false;

        protected Vector2 location;

        protected Vector2 maximumSize;

        protected Vector2 minimumSize;

        protected static Control selectedControl;

        protected Vector2 size;
        #endregion

        #region Event
        public event EventHandler Click;

        public event EventHandler<MonoKeyEventArg> KeyPress;

        public event EventHandler<MonoKeyEventArg> KeyRelease;

        public event EventHandler MouseDoubleClick;

        public event EventHandler<MonoMouseEventArg> MouseDown;

        public event EventHandler MouseEnter;

        public event EventHandler MouseHover;

        public event EventHandler MouseLeave;

        public event EventHandler MouseMoved;

        public event EventHandler<MonoMouseEventArg> MousePressing;

        public event EventHandler<MonoMouseEventArg> MouseUp;

        public event EventHandler<MonoDimensionEventArg> Move;

        public event EventHandler<MonoPaintEventArg> Paint;

        public event EventHandler<MonoPaintEventArg> PaintBackground;


        public event EventHandler<MonoDimensionEventArg> Resize;

        public event EventHandler<MonoMouseEventArg> Scroll;
        #endregion

        #region ControlProperty
        public Rectangle ControlShape
        {
            get => controlShape;
        }
        #endregion

        #region ControlPrivateProperty
        private Rectangle controlShape;

        private Texture2D texture;

        private bool wasOnHover = false;
        #endregion

        public Control()
        {
            #region Properties
            Alpha = 1;
            Cursor = Game.DefaultCursor;
            Enabled = true;
            location = new Vector2(0, 0);
            maximumSize = new Vector2(1000, 1000);
            minimumSize = new Vector2(1, 1);
            size = new Vector2(100, 20);
            Visible = true;
            #endregion

            #region Control
            controlShape = new Rectangle((int)Location.X, (int)Location.Y, (int)Size.X, (int)Size.Y);

            texture = new Texture2D(Game.Instance.GraphicsDevice, (int)Size.X, (int)Size.Y);
            texture.Fill();
            #endregion

            #region Events
            MouseDown += new EventHandler<MonoMouseEventArg>((sender, e) => {
                if (!Selected)
                {
                    if (selectedControl != null)
                        selectedControl.Selected = false;

                    Selected = true;
                    selectedControl = (Control)sender;
                }
            });
            MouseEnter += new EventHandler((sender, e) => { Game.CurrentCursor = Cursor; isHovering = true; });
            MouseLeave += new EventHandler((sender, e) => { Game.CurrentCursor = Game.DefaultCursor; isHovering = false; });
            #endregion
        }

        public virtual void Update()
        {
            if (Visible && Enabled)
            {
                if (Input.MouseShape.Intersects(ControlShape))
                {
                    if (!wasOnHover)
                        MouseEnter?.Invoke(this, null);

                    MouseHover?.Invoke(this, null);

                    if (Input.DoubleClick)
                        MouseDoubleClick?.Invoke(this, null);
                    if (Input.Left_MouseDown || Input.Right_MouseDown)
                    {
                        MouseDown?.Invoke(this, new MonoMouseEventArg(Input.Left_MouseDown, Input.Right_MouseDown));
                        Click?.Invoke(this, null);
                    }
                    else if (Input.Left_MousePressing || Input.Right_MousePressing)
                        MousePressing?.Invoke(this, new MonoMouseEventArg(Input.Left_MousePressing, Input.Right_MousePressing));
                    else if (Input.Left_MouseUp || Input.Right_MouseUp)
                        MouseUp?.Invoke(this, new MonoMouseEventArg(Input.Left_MouseUp, Input.Right_MouseUp));

                    if (Input.MouseMoved)
                        MouseMoved?.Invoke(this, null);

                    if (Input.MouseScrollDiff != 0)
                        Scroll?.Invoke(this, new MonoMouseEventArg(Input.MouseScroll));

                    wasOnHover = true;
                }
                else
                {
                    if (wasOnHover)
                    {
                        MouseLeave?.Invoke(this, null);

                        wasOnHover = false;
                    }
                }

                if (Selected)
                {
                    if (Input.KeyDown(Keys.Space))
                        Click?.Invoke(this, null);

                    Keys[] k = Input.PressedKeys.Where(key => !Input.LastPressedKeys.Any(lKey => lKey == key)).ToArray();
                    for (int i = 0; i < k.Length; i++)
                        KeyPress?.Invoke(this, new MonoKeyEventArg(k[i]));
                    k = null;

                    k = Input.LastPressedKeys.Where(lKey => !Input.PressedKeys.Any(key => key == lKey)).ToArray();
                    for (int i = 0; i < k.Length; i++)
                        KeyRelease?.Invoke(this, new MonoKeyEventArg(k[i]));
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                PaintBackground?.Invoke(this, new MonoPaintEventArg(spriteBatch));

                if (BackgroundImage != null)
                    spriteBatch.Draw(BackgroundImage, controlShape, controlShape, Color.White * Alpha);

                spriteBatch.Draw(texture, controlShape, ((Selected && SelectedColor != Color.Transparent) ? SelectedColor : ((isHovering && HoverColor != Color.Transparent) ? HoverColor : (invertColors ? ForeColor : BackColor))) * Alpha);

                Paint?.Invoke(this, new MonoPaintEventArg(spriteBatch));
            }
        }
    }
}