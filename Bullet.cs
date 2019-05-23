using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Explore.Particles;
using System.Collections.Generic;
using System;

namespace Explore
{
    public class Bullet : GameObject
    {
        protected int width = 16;
        protected int height = 8;

        protected int speed = 1200;

        protected float lifetime = 1;

        protected int damage;

        public int Damage {
            get {
                return damage;
            }
        }

        protected Random rand = new Random();

        public Bullet(Vector2 _position, int direction) : base(_position) {
            position = _position;
            speed *= direction;
            damage = Config.Bullet["damage"].IntValue;
        }

        public Bullet(Vector2 _position) : base(_position) {
            position = _position;
            damage = Config.Bullet["damage"].IntValue;
        }

        public override void Update() {

            if (!isDead) {

                rectangle = Helper.MakeRectangle(position, width, height);
               
                position.X += speed * GameManager.DeltaTime;

                lifetime -= GameManager.DeltaTime;

                if (lifetime <= 0) {
                    isDead = true;
                }
            }
        }
        protected void CheckForPlatformCollision() {
            for (int i = 0; i < GameManager.platforms.Count; i++) {
                if (Helper.RectRect(rectangle, GameManager.platforms[i].Rectangle)) {
                    isDead = true;
                    Effects.Explosion(position);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

public class Rocket : Bullet {

        private ParticleSystem fx;

        public Rocket(Vector2 _position) : base(_position) {
            position = _position;
            speed = 1000;

            width = 12;
            height = 20;

            damage = Config.Bullet["rocketDamage"].IntValue;

            fx = new ParticleSystem(new Settings() {
                number_per_frame = rand.Next(5),
                size = new Point(2, 5),
                speed = 600,
                lifespan = 0.07f,
                accX = new Vector2(-1, 1),
                accY = new Vector2(2, 20),
                color = new List<Color>() {
                    Color.Red,
                    Color.Orange,
                    Color.Yellow,
                    Color.White
                }
            }, rectangle);
        }

        public override void SetTexture(Texture2D _texture) {
            texture = _texture;
            fx.SetTexture();
        }

        public override void Update() {
            if (!isDead) {

                rectangle = Helper.MakeRectangle(position, width, height);
               
                position.Y -= speed * GameManager.DeltaTime;

                lifetime -= GameManager.DeltaTime;

                if (lifetime <= 0) {
                    isDead = true;
                }

                CheckForPlatformCollision();
            }

            fx.rectangle = new Rectangle(rectangle.X, rectangle.Bottom, rectangle.Width, 10);

            fx.Update();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
            fx.Draw(spriteBatch);
        }
    }

    public class RedBullet : Bullet {
        private Color color;
        public RedBullet(Vector2 position) : base(position) {
            width = 8;
            height = 16;
            speed = 1000;
            damage = Config.Bullet["damage"].IntValue;
            color = Color.Red;
        }

        public override void Update() {
            if (!isDead) {
                position.Y += speed * GameManager.DeltaTime;
                rectangle = Helper.MakeRectangle(position, width, height);
                CheckForPlatformCollision();
                lifetime -= GameManager.DeltaTime;
                if (lifetime <= 0) {
                    isDead = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, color, MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Width / 2), new Vector2(1, 1), SpriteEffects.None, 0);
        }
    }
}