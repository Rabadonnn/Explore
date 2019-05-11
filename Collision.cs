using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Explore
{
    public static class Helper
    {
        public enum Collision {
            Top,
            Bottom,
            Left,
            Right,
            NoCollision
        };

        public static bool RectRect(Rectangle r1, Rectangle r2) {
            if (r1.X + r1.Width >= r2.X && r1.X <= r2.X + r2.Width &&
                r1.Y + r1.Height >= r2.Y && r1.Y <= r2.Y + r2.Height) {
                return true;
            } else {
                return false;
            }
        }

        public static Collision RectRectExtended(Rectangle r1, Rectangle r2) {

            if (Helper.RectRect(r1, r2)) {
                if (r1.Center.Y < r2.Center.Y) {
                    // Collision on bottom
                    return Collision.Bottom;
                } else if (r1.Center.Y > r2.Center.Y) {
                    // Collision on Top
                    return Collision.Top;
                } else if (r1.Center.X > r2.Center.X) {
                    // Collision on Right
                    return Collision.Right;
                } else if (r1.Center.X < r2.Center.X) {
                    // Collision On Left
                    return Collision.Left;
                } else {
                    return Collision.NoCollision;
                }
            } else {
                return Collision.NoCollision;
            }
        }

        public static Collision RectangleCollision(Rectangle r1, Rectangle r2) {
            Collision result;

            float w = 0.5f * (r1.Width + r2.Width);
                float h = 0.5f * (r1.Height + r2.Height);
                float dx = r1.Center.X - r2.Center.X;
                float dy = r1.Center.Y - r2.Center.Y;

                if (Math.Abs(dx) <= w && Math.Abs(dy) <= h) {
                    float wy = w * dy;
                    float hx = h * dx;

                    if (wy > hx) {
                        if (wy > -hx) {
                           result = Collision.Top;
                        } else {
                            result = Collision.Left;
                        }
                    } else {
                        if (wy > -hx) {
                            result = Collision.Right;
                        } else {
                            result = Collision.Bottom;
                        }
                    }

                } else {
                    result = Collision.NoCollision;
                }

            return result;
        }

        public static double Distance(Vector2 v1, Vector2 v2) {
            return Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2));
        }

        public static float MapValue(float value, float start1, float stop1, float start2, float stop2) {
            return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        }

        static public void DrawString(SpriteBatch spriteBatch, SpriteFont font, string strToDraw, Color _color, Rectangle boundaries)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaires.
            float scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
            position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, _color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }
    }
}