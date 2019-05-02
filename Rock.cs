using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Explore
{
    public class Rock : GameObject
    {
        private Vector2 size;
        private Vector2 target;
        private Vector2 velocity;
        private float speed = 1500;
        public float lifeTime = 0.5f;
        private float rotation;
        private Random rand = new Random();

        public Rock(Vector2 _target, Vector2 _position, Vector2 _size) : base("rock") {
            target = _target;
            position = _position;
            size = _size;
            rectangle = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2), (int)size.X, (int)size.Y);
            velocity = target - position;
            if (velocity.X != 0 && velocity.Y != 0) {
               velocity.Normalize();
            }
            rotation = rand.Next(-314, 314) / 100;
        }

        public void Update() {

            if (!isDead) {
                position += velocity * speed * GameManager.DeltaTime;
                rectangle = new Rectangle((int)(position.X - size.X / 2), (int)(position.Y - size.Y / 2), (int)size.X, (int)size.Y);

                for (int i = 0; i < GameObject.GetObjects("platform").Count; i++) {
                    GameObject obj = GameObject.GetObjects("platform")[i];
                    if (Collision.RectRect(rectangle, obj.rectangle)) {
                        isDead = true;
                    }
                }

                lifeTime -= GameManager.DeltaTime;

                if (lifeTime <= 0) {
                    isDead = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture: texture, destinationRectangle: rectangle, color: Color.White);
        }
    }
}