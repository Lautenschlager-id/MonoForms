using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoForms
{
    static class Input
    {
        #region Property
        // Mouse
        public static bool DoubleClick
        {
            get => MouseLocation == lastClickCoordinate && (DateTime.Now - lastClickTime).TotalSeconds < doubleClickDelay && Left_MouseUp;
        }

        public static bool Left_MouseDown
        {
            get => lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool Left_MousePressing
        {
            get => lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool Left_MouseUp
        {
            get
            {
                bool released = lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released;

                if (released)
                {
                    lastClickCoordinate = MouseLocation;
                    lastClickTime = DateTime.Now;
                }

                return released;
            }
        }

        public static Vector2 MouseLocation
        {
            get => new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        public static bool MouseMoved
        {
            get => new Vector2(lastMouseState.X, lastMouseState.Y) != MouseLocation;
        }

        public static int MouseScroll
        {
            get => currentMouseState.ScrollWheelValue;
        }

        public static int MouseScrollDiff
        {
            get
            {
                if (lastMouseState.ScrollWheelValue == currentMouseState.ScrollWheelValue) return 0;
                return currentMouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue ? 1 : -1;
            }
        }

        public static Rectangle MouseShape { get; private set; }

        public static bool Right_MouseDown
        {
            get => lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool Right_MousePressing
        {
            get => lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool Right_MouseUp
        {
            get => lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released;
        }

        // Keyboard
        public static bool HoldingKey(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool KeyDown(Keys key)
        {
            return lastKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);
        }

        public static bool KeyUp(Keys key)
        {
            return lastKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        public static Keys[] PressedKeys
        {
            get => currentKeyboardState.GetPressedKeys();
        }

        public static Keys[] LastPressedKeys { get; private set; }
        #endregion

        #region PrivateProperty
        // Mouse
        private static MouseState currentMouseState;

        private const float doubleClickDelay = .25f;

        private static Vector2 lastClickCoordinate;

        private static DateTime lastClickTime;

        private static MouseState lastMouseState;

        // Keyboard
        private static KeyboardState currentKeyboardState, lastKeyboardState;
        #endregion

        public static void Update()
        {
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            LastPressedKeys = PressedKeys;
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            MouseShape = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);
        }
    }
}
