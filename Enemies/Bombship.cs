using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Explore.Bullets;

namespace Explore.Enemies
{
    public class Bombship : BaseShip
    {
        private float initialShootingCooldown;
        private float shootingCooldown;
        private List<Bullet> bullets;
        private int range;

        public Bombship(Vector2 _position) : base(_position) {
            lerpSpeed = Config.BombShip["lerpSpeed"].FloatValue;
            health = Config.BombShip["health"].IntValue;
            initialShootingCooldown = Config.BombShip["shootingCooldown"].FloatValue;
            bullets = new List<Bullet>();
            scale = new Vector2(1, 1);
            range = Config.BombShip["range"].IntValue;
        }

        public override void SetTexture() {
            texture = GameManager.Assets["bombship"];
            healthBar.SetTexture();
        }

        public override void Update() {
            if (Helper.Distance(position, checkPoint) < range) {
                CheckForShooting();
                if (Helper.Distance(position, checkPoint) < 25) {
                    checkPoint = MakeNewCheckPoint();
                }
            }

            position = Vector2.Lerp(position, checkPoint, lerpSpeed);

            UpdateRectangle();
            CheckCollisions();
            CheckLife();
            UpdateHealthBar(Config.BombShip["health"].IntValue);

            foreach (var b in bullets.ToArray()) {
                if (b.isDead) {
                    bullets.Remove(b);
                } else {
                    b.Update();

                    if (Helper.RectRect(b.Rectangle, GameManager.player.Rectangle)) {
                        b.isDead = true;
                        GameManager.player.Hit((b as RedBullet).Damage);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Width / 2), scale, SpriteEffects.None, 0);
            healthBar.Draw(spriteBatch);

            foreach (var b in bullets) {
                b.Draw(spriteBatch);
            }
        }

        private void CheckForShooting() {
            if (shootingCooldown <= 0) {
                var b = new RedBullet(position);
                (b as RedBullet).SetTexture();
                bullets.Add(b);
                shootingCooldown = initialShootingCooldown;
            } else {
                shootingCooldown -= GameManager.DeltaTime;;
            }
        }
    }
}