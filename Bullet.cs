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

        protected string tag;

        protected Random rand = new Random();

        public Bullet(Vector2 _position, int direction, string _tag) : base("bullet") {
            position = _position;
            speed *= direction;
            tag = _tag;
        }

        public Bullet(Vector2 _position, string _tag) : base("bullet") {
            position = _position;
            tag = _tag;
        }

        public override void Update() {

            if (!isDead) {

                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
               
                position.X += speed * GameManager.DeltaTime;

                lifetime -= GameManager.DeltaTime;

                if (lifetime <= 0) {
                    isDead = true;
                }


                List<Platform> platforms = GameManager.platforms;

                for (int i = 0; i < platforms.Count; i++) {
                    if (Helper.RectRect(rectangle, platforms[i].rectangle)) {
                        isDead = true;
                    }
                }
            }
        }
        protected void CheckForPlatformCollision() {
            for (int i = 0; i < GameManager.platforms.Count; i++) {
                if (Helper.RectRect(rectangle, GameManager.platforms[i].rectangle)) {
                    isDead = true;
                    Explosions.Explosion(position);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

public class Rocket : Bullet {

        private ParticleSystem fx;

        public Rocket(Vector2 _position, string _tag) : base(_position, _tag) {
            position = _position;
            tag = _tag;
            speed = 1000;

            width = 12;
            height = 20;

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

                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
               
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

    public class Bomb : Bullet {
        public Bomb(Vector2 _position) : base(_position, "ship") {
            position = _position;
            speed = 850;
            width = 32;
            height = 32;
        }

        public void SetTexture() {
            texture = GameManager.Assets["bomb"];
        }

        public override void Update() {
            if (!isDead) {

                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
               
                position.Y += speed * GameManager.DeltaTime;

                lifetime -= GameManager.DeltaTime;

                if (lifetime <= 0) {
                    isDead = true;
                }

                CheckForPlatformCollision();
            }
        }
    }
}