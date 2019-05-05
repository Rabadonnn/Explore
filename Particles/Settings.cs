using Microsoft.Xna.Framework;

namespace Explore.Particles
{
    public class Settings
    {
        public Settings() {}
        public int number_per_frame = 10;
        public int size = 10;
        public int speed = 15;
        public float lifespan = 20;
        public Vector2 velocity = new Vector2(0, 0);
        public Vector2 accX = new Vector2(-1, 1);
        public Vector2 accY = new Vector2(-7, -2);
        public float gravity = 0.1f;
        public Color color = new Color(255, 255, 255);
    }
}