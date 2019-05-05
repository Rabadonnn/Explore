using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore
{
    public class Bullet : GameObject
    {
        private int width = 16;
        private int height = 8;

        private int speed = 1200;

        private float lifetime = 1;

        public Bullet(Vector2 _position, int direction) : base("bullet") {
            position = _position;
            speed *= direction;
        }

        public void Update() {

            if (!isDead) {

                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
               
                position.X += speed * GameManager.DeltaTime;

                lifetime -= GameManager.DeltaTime;

                if (lifetime <= 0) {
                    isDead = true;
                }


                System.Collections.Generic.List<Platform> platforms = GameManager.platforms;

                for (int i = 0; i < platforms.Count; i++) {
                    if (Helper.RectRect(rectangle, platforms[i].rectangle)) {
                        isDead = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}