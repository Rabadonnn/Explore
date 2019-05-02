using Microsoft.Xna.Framework;
using System;

namespace Explore
{
    public static class Collision
    {
        public static bool RectRect(Rectangle r1, Rectangle r2) {
            if (r1.X + r1.Width >= r2.X && r1.X <= r2.X + r2.Width &&
                r1.Y + r1.Height >= r2.Y && r1.Y <= r2.Y + r2.Height) {
                return true;
            } else {
                return false;
            }
        }

        public static double Distance(Vector2 v1, Vector2 v2) {
            return Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2));
        }

        public static float MapValue(float value, float start1, float stop1, float start2, float stop2) {
            return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        }
    }
}