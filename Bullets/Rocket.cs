using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Explore.Particles;
using System.Collections.Generic;
using System;

namespace Explore.Bullets
{
    // Rachetele folosite de lansatorul de rachete al jucatorului

    public class Rocket : Bullet {

        private ParticleSystem fx;

        public Rocket(Vector2 _position) : base(_position) {
            position = _position;
            speed = 1000;

            width = 12;
            height = 20;

            damage = Config.Bullet["rocketDamage"].IntValue;

            fx = new ParticleSystem(new particleSettings() {
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

        public override void SetTexture() {
            texture = GameManager.Assets["rpg_ammo"];
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
}