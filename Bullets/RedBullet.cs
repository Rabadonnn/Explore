using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Explore.Particles;
using System.Collections.Generic;
using System;

namespace Explore.Bullets
{
    // La fel ca un glont normal doar ca are o nuanta de rosu, este indreptat in jos si este folosit doar de un tip de nava

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