using Microsoft.Xna.Framework.Input;

namespace Explore
{
    public static class Input
    {
        public static bool Up {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.W);
            }
        }
        public static bool Down {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.S);
            }
        }
        public static bool Right {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.D);
            }
        }
        public static bool Left {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.A);
            }
        }
        public static bool RightClick {
            get {
                return Mouse.GetState().RightButton == ButtonState.Pressed;
            }
        }
        public static bool LeftClick {
            get {
                return Mouse.GetState().LeftButton == ButtonState.Pressed;
            }
        }
        public static bool Respawn {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.P);
            }
        }

        public static float MouseX {
            get {
                return Mouse.GetState().Position.X;
            }
        }

        public static float MouseY {
            get {
                return Mouse.GetState().Position.Y;
            }
        }
    }
}