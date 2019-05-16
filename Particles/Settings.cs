using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Explore.Particles
{
    public class Settings
    {
        public Settings() {}
        public int number_per_frame = 10;
        public Point size = new Point(10, 10);
        public int speed = 15;
        public float lifespan = 20;
        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 accX = new Vector2(-1, 1);
        public Vector2 accY = new Vector2(-7, -2);
        public float gravity = 0.1f;
        public List<Color> color = new List<Color>() { new Color(255, 255, 255) };
        public bool oneTime = false;

        private void SwapValues(object a, object b) {
            var t = a;
            a = b;
            b = t;
        }

        public void CheckValues() {
            if (accX.X > accX.Y) {
                SwapValues(accX.X, accX.Y);
            }

            if (accY.X > accY.Y) {
                SwapValues(accY.X, accY.Y);
            }
        }
    }
}