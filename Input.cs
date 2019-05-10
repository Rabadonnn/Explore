using Microsoft.Xna.Framework.Input;

namespace Explore
{
    public static class Input
    {
        public static bool Up {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up);
            }
        }
        public static bool Down {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down);
            }
        }
        public static bool Right {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            }
        }
        public static bool Left {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
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

        public static bool Space {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.Space);
            }
        }
        
        public static bool Q {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.Q);
            }
        }

        public static bool E {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.E);
            }
        }

        public static bool V {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.V);
            }
        }
    }
}