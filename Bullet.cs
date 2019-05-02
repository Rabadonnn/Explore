using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore
{
    public class Bullet : GameObject
    {
        private float lifetime = 2;
        private int speed = 700;
        private int direction;
        private int width = 8;
        private int height = 4;

        public Bullet(Vector2 _position, int _direction) : base("bullet") {
            position = _position;
            direction = _direction;
        }

        public void Update() {
            position.X += speed * direction * GameManager.DeltaTime;
            rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
            lifetime -= GameManager.DeltaTime;
            if (lifetime < 0) {
                isDead = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}