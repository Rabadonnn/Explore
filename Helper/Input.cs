using Microsoft.Xna.Framework.Input;

namespace Explore
{
    public static class Input
    {
        private static bool gamePadConnected = false;
        private static GamePadState state;

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
                if (gamePadConnected) {
                    return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right) || state.ThumbSticks.Left.X > 0;
                }
                return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            }
        }
        public static bool Left {
            get {
                if (gamePadConnected) {
                    return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left) || state.ThumbSticks.Left.X < 0;
                }
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
                if (gamePadConnected) {
                    return Keyboard.GetState().IsKeyDown(Keys.Space) || state.Buttons.A == ButtonState.Pressed;
                }
                return Keyboard.GetState().IsKeyDown(Keys.Space);
            }
        }
        
        public static bool Q {
            get {
                if (gamePadConnected) {
                    return Keyboard.GetState().IsKeyDown(Keys.Q) || state.Buttons.B == ButtonState.Pressed;
                }
                return Keyboard.GetState().IsKeyDown(Keys.Q);
            }
        }

        public static bool E {
            get {
                if (gamePadConnected) {
                    return Keyboard.GetState().IsKeyDown(Keys.E) || state.Buttons.X == ButtonState.Pressed;
                }
                return Keyboard.GetState().IsKeyDown(Keys.E);
            }
        }

        public static bool V {
            get {
                if (gamePadConnected) {
                    return Keyboard.GetState().IsKeyDown(Keys.V) || state.Buttons.Y == ButtonState.Pressed;
                }
                return Keyboard.GetState().IsKeyDown(Keys.V);
            }
        }

        public static bool LeftClickReleased {
            get {
                return Mouse.GetState().LeftButton == ButtonState.Released;
            }
        }

        public static bool Esc {
            get {
                return Keyboard.GetState().IsKeyDown(Keys.Escape);
            }
        }

        public static void UpdateGamePad() {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(Microsoft.Xna.Framework.PlayerIndex.One);

            if (capabilities.IsConnected) {
                gamePadConnected = true;
                state = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            }
        }
    }
}