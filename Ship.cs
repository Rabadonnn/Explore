using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;


namespace Explore
{
    public class BaseShip : GameObject {

        private int health;
        private int previousHealth;

        private Vector2 checkPoint;
        private Random rand;
        private Vector2 scale = new Vector2(2, 2);

        private int baseEnemyDropChance = 50;

        private Color color;

        public BaseShip(Vector2 _position) {

            health = 10;
            previousHealth = health;

            position = _position;
            rand = new Random();
            checkPoint = MakeNewCheckPoint();

            color = Color.White;
        }

        public void SetTexture() {
            texture = GameManager.Assets["dropship"];
        }

        public void Update() {
            if (Helper.Distance(position, checkPoint) < 25) {
                checkPoint = MakeNewCheckPoint();

                if (IsAbovePlatform()) {
                    Drop();
                }

            } else {
                position = Vector2.Lerp(position, checkPoint, 0.01f);
            }


            int rectangleX = (int)(position.X - (texture.Width / 2) * scale.X);
            int rectangleY = (int)(position.Y - (texture.Height / 2) * scale.Y);
            int rectangleW = (int)(texture.Width * scale.X);
            int rectangleH = (int)(texture.Height * scale.Y);
            rectangle = new Rectangle(rectangleX, rectangleY, rectangleH, rectangleH);

            List<Rocket> rockets = GameManager.player.Rockets;

            for (int i = 0; i < rockets.Count; i++) {
                if (Helper.RectRect(rectangle, rockets[i].rectangle)) {
                    health -= 5;
                    rockets[i].isDead = true;
                }
            }

            for (int i = 0; i < GameManager.player.Bullets.Count; i++) {
                if (Helper.RectRect(rectangle, GameManager.player.Bullets[i].rectangle)) {
                    health -= 2;
                    GameManager.player.Bullets[i].isDead = true;
                }
            }

            if (health < previousHealth) {
                color = Color.Red;
            } else {
                color = Color.Red;
            }

            if (health < 1) {
                isDead = true;
                GameManager.player.DestroyedShip();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
        }

        private void Drop() {
            int randInt = rand.Next(100);

            if (randInt < baseEnemyDropChance) {
                BaseEnemy e = new BaseEnemy(position);
                e.SetAnimations();
                WaveManager.AddBaseEnemy(e);
            }
        }

        private bool IsAbovePlatform() {
            bool result = false;

            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                if (result) {
                    break;
                } else {
                    if (position.X > platforms[i].rectangle.Left &&
                        position.X < platforms[i].rectangle.Right && 
                        position.Y < platforms[i].rectangle.Top) {
                            result = true;
                        }
                }
            }

            return result;
        }

        private Vector2 MakeNewCheckPoint() {
            float x = rand.Next((int)GameManager.player.Position.X - 500, (int)GameManager.player.Position.X + 500);
            return new Vector2(x, rand.Next(-GameManager.ScreenHeight, 0));
        }
    }
}