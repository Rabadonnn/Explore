using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore.Paritcles
{
    public class Particle
    {
        private Settings settings;
        public Rectangle rectangle;
        private Vector2 position;
        private Vector2 acceleration;
        private Vector2 velocity;
        private float gravity;
        private int speed;
        private float lifespan;
        public Color color;
        private int size;
        public bool IsDead {
            get {
                return lifespan <= 0;
            }
        }
        
        private Random rand = new Random();

        public Particle (Settings _settings, Vector2 _position) {
            settings = _settings;
            position = _position;
            acceleration = new Vector2(rand.Next((int)settings.accX.X, (int)settings.accX.Y), rand.Next((int)settings.accY.X, (int)settings.accY.Y));
            velocity = settings.velocity;
            speed = settings.speed;
            lifespan = settings.lifespan;
            color = settings.color;
            size = settings.size;
            gravity = settings.gravity;
        }

        public void Update() {
            acceleration.Y += gravity;
            velocity += acceleration;
            velocity.Normalize();
            position += velocity * GameManager.DeltaTime * speed;
            rectangle = new Rectangle((int)(position.X - size / 2), (int)(position.Y - size / 2), size, size);
            lifespan -= GameManager.DeltaTime;
        }
    }
}