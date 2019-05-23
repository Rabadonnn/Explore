using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Explore.Particles;
using System.Collections.Generic;
using System;

namespace Explore.Bullets
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

        public virtual void SetTexture() {
            texture = GameManager.Assets["bullet"];
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
}