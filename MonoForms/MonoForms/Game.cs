using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoForms
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static readonly Random random = new Random();

        public static Game Instance { get; private set; }
        public GameTime GameTime { get; private set; }

        public static MonoSpriteFont SystemFont { get; private set; }
        public static MonoSpriteFont DefaultFont { get; private set; }
        public static Texture2D DefaultCursor { get; private set; }

        public static Texture2D CurrentCursor;
        public static float CursorAlpha = 1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Instance = this;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LibContent.LoadContent(Content);

            SystemFont = Tools.GenerateSpriteFont(
                fontName: "Verdana",
                size: 8,
                characterRegions: new System.Collections.Generic.List<Point>() {
                    new Point(9650, 9650), // ▲
                    new Point(9660, 9660), // ▼
                    new Point(9679, 9679), // ●
                    new Point(10004, 10004), // ✔
                }
            );

            DefaultFont = Tools.GenerateSpriteFont(
                fontName: "Verdana",
                size: 8.75f,
                characterRegions: new System.Collections.Generic.List<Point>() { new Point(32, 255) }
            );

            DefaultCursor = CurrentCursor = LibContent.CursorArrow;
        }

        static class Test
        {
            static PictureBox pictureBox;
            static Label label;
            static LinkLabel linkLabel;
            static Button button;
            static ComboBox comboBox;
            static ListBox listBox;
            static NumericUpDown numericUpDown;
            static ProgressBar progressBar;
            static TrackBar trackBar;
            static Timer timer;
            static CheckBox checkBox;
            static RadioButton radioButton;
            static TextBox textBox;

            static Test()
            {
                pictureBox = new PictureBox()
                {
                    AutoSize = true,
                    Image = LibContent.Image
                };

                label = new Label()
                {
                    AutoSize = true,
                    GrowVertically = true,
                    Text = "I don't know the real name of my friend because he is a dick and we don't talk anymore but I think it is someting like kdjsaflkjaf.",
                    Location = new Vector2(120, 0)
                };

                linkLabel = new LinkLabel()
                {
                    Text = "Click me!",
                    Link = "https://www.facebook.com/messages/t/rafaela.salutes",
                    Location = new Vector2(220, 0)
                };

                button = new Button()
                {
                    Text = "Send mortal virus that is going to kill the world",
                    Location = new Vector2(320, 0),
                    HoverColor = Color.Gainsboro,
                };

                comboBox = new ComboBox()
                {
                    Location = new Vector2(0, 150),
                };
                comboBox.AddItem("banana");
                comboBox.AddItem("apple");
                comboBox.AddItem("pineapple");
                comboBox.AddItem("grape");
                comboBox.AddItem("orange");

                listBox = new ListBox(true)
                {
                    Location = new Vector2(150, 150),
                    MaxLength = 8
                };
                listBox.AddItem("banana");
                listBox.AddItem("apple");
                listBox.AddItem("pineapple");
                listBox.AddItem("grape");
                listBox.AddItem("orange");

                numericUpDown = new NumericUpDown()
                {
                    Location = new Vector2(300, 150)
                };

                progressBar = new ProgressBar()
                {
                    ForeColor = Color.RoyalBlue,
                    Location = new Vector2(5, 300),
                };
                progressBar.MousePressing += new EventHandler<MonoMouseEventArg>((a, b) => progressBar.PerformStep());

                trackBar = new TrackBar(Enums.TrackbarOrientation.Vertical)
                {
                    Location = new Vector2(550, 150),
                    LineThickness = 20,
                    BackColor = Color.Turquoise,
                    ForeColor = Color.LightGoldenrodYellow,
                    HoverColor = Color.LightCoral,
                };
                trackBar.ValueChanged += new EventHandler<MonoValueEventArg>((a, b) => Console.WriteLine(trackBar.Value));

                timer = new Timer()
                {
                    Enabled = true,
                    Interval = 2500
                };
                timer.Tick += new EventHandler((a, b) => Console.WriteLine(timer.Interval));

                checkBox = new CheckBox()
                {
                    Location = new Vector2(590, 150),
                    Text = "Select me or else"
                };

                radioButton = new RadioButton()
                {
                    Location = new Vector2(590, 190),
                    Text = "Select me or else"
                };

                textBox = new TextBox()
                {
                    Location = new Vector2(20, 360),
                    AcceptsTab = true
                };
            }

            public static void Update()
            {
                pictureBox.Update();
                label.Update();
                linkLabel.Update();
                button.Update();
                comboBox.Update();
                listBox.Update();
                numericUpDown.Update();
                progressBar.Update();
                trackBar.Update();
                timer.Update();
                checkBox.Update();
                radioButton.Update();
                textBox.Update();
            }

            public static void Draw(SpriteBatch spriteBatch)
            {
                pictureBox.Draw(spriteBatch);
                label.Draw(spriteBatch);
                linkLabel.Draw(spriteBatch);
                button.Draw(spriteBatch);
                comboBox.Draw(spriteBatch);
                listBox.Draw(spriteBatch);
                numericUpDown.Draw(spriteBatch);
                progressBar.Draw(spriteBatch);
                trackBar.Draw(spriteBatch);
                checkBox.Draw(spriteBatch);
                radioButton.Draw(spriteBatch);
                textBox.Draw(spriteBatch);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                GameTime = gameTime;

                Input.Update();

                Test.Update();
            
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Test.Draw(spriteBatch);

            if (CurrentCursor != null)
                spriteBatch.Draw(CurrentCursor, Input.MouseLocation, Color.White * CursorAlpha);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
