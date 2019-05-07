using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore
{
    public class Bullet : GameObject
    {
        protected int width = 16;
        protected int height = 8;

        protected int speed = 1200;

        protected float lifetime = 1;

        protected string tag;

        public Bullet(Vector2 _position, int direction, string _tag) : base("bullet") {
            position = _position;
            speed *= direction;
            tag = _tag;
        }

        public Bullet(Vector2 _position, string _tag) : base("bullet") {
            position = _position;
            tag = _tag;
        }

        public virtual void Update() {

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

    public class Rocket : Bullet {
        public Rocket(Vector2 _position, string _tag) : base(_position, _tag) {
            position = _position;
            tag = _tag;
            speed = 1000;

            width = 12;
            height = 20;
        }

        public override void Update() {
            if (!isDead) {

                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
               
                position.Y -= speed * GameManager.DeltaTime;

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
    }
}