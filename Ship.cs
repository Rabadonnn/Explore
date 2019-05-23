using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Explore
{
    public class BaseShip : GameObject {

        protected int health;

        protected Vector2 checkPoint;
        protected Random rand;
        protected Vector2 scale = new Vector2(2, 2);

        protected int baseEnemyDropChance;

        protected float lerpSpeed;

        protected HealthBar healthBar;

        public BaseShip(Vector2 _position) {

            health = Config.BaseShip["health"].IntValue;
            baseEnemyDropChance = Config.BaseShip["baseEnemyDropChance"].IntValue;

            position = _position;
            rand = new Random();
            checkPoint = MakeNewCheckPoint();
            lerpSpeed = Config.BaseShip["lerpSpeed"].FloatValue;

            healthBar = new HealthBar();
        }

        public virtual void SetTexture() {
            texture = GameManager.Assets["dropship"];
            healthBar.SetTexture();
        }

        public override void Update() {
            if (Helper.Distance(position, checkPoint) < 25) {
                checkPoint = MakeNewCheckPoint();

                if (IsAbovePlatform()) {
                    Drop();
                }

            } else {
                position = Vector2.Lerp(position, checkPoint, lerpSpeed);
            }

            UpdateRectangle();

            CheckCollisions();

            CheckLife();

            UpdateHealthBar(Config.BaseShip["health"].IntValue);
        }

        protected void UpdateHealthBar(int maxHealth) {
            healthBar.Update(new Vector2(position.X, position.Y - 50), maxHealth, health);
        }

        protected void CheckLife() {
            if (health < 1) {
                isDead = true;
                GameManager.player.DestroyedShip();
            }
        }
        

        protected void CheckCollisions() {
            foreach (var p in GameManager.player.Projectiles) {
                if (Helper.RectRect(rectangle, p.Rectangle)) {
                    if (p is Rocket) {
                        health -= (p as Rocket).Damage;
                    } else if (p is Bullet) {
                        health -= (p as Bullet).Damage;
                    }
                    Effects.BigExplosion(position);
                    p.isDead = true;
                }
            }
        }

        protected void UpdateRectangle() {
            int rectangleX = (int)(position.X - (texture.Width / 2) * scale.X);
            int rectangleY = (int)(position.Y - (texture.Height / 2) * scale.Y);
            int rectangleW = (int)(texture.Width * scale.X);
            int rectangleH = (int)(texture.Height * scale.Y);
            rectangle = new Rectangle(rectangleX, rectangleY, rectangleH, rectangleH);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
            healthBar.Draw(spriteBatch);
        }

        private void Drop() {
            int randInt = rand.Next(100);

            if (randInt < baseEnemyDropChance) {
                BaseEnemy e = new BaseEnemy(position);
                e.SetAnimations();
                WaveManager.AddBaseEnemy(e);
            }
        }

        protected bool IsAbovePlatform() {
            bool result = false;

            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                if (result) {
                    break;
                } else {
                    if (position.X > platforms[i].Rectangle.Left &&
                        position.X < platforms[i].Rectangle.Right && 
                        position.Y < platforms[i].Rectangle.Top) {
                            result = true;
                        }
                }
            }

            return result;
        }

        protected Vector2 MakeNewCheckPoint() {
            float x = rand.Next((int)GameManager.player.Position.X - 500, (int)GameManager.player.Position.X + 500);
            return new Vector2(x, rand.Next(-GameManager.ScreenHeight, 0));
        }
    }
}