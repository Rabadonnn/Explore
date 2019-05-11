using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;


namespace Explore
{
    public class BaseShip : GameObject {

        private int health;

        private Vector2 checkPoint;
        private Random rand;
        private Vector2 scale = new Vector2(2, 2);

        private int baseEnemyDropChance = 50;

        private List<BaseEnemy> baseEnemies;

        public BaseShip(Vector2 _position) {

            health = 1;

            position = _position;
            rand = new Random();
            checkPoint = MakeNewCheckPoint();

            baseEnemies = new List<BaseEnemy>();
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


            for (int i = 0; i < baseEnemies.Count; i++) {
                if (!baseEnemies[i].isDead) {
                    baseEnemies[i].Update();
                } else {
                    baseEnemies.RemoveAt(i);
                }
            }

            int rectangleX = (int)(position.X - (texture.Width / 2) * scale.X);
            int rectangleY = (int)(position.Y - (texture.Height / 2) * scale.Y);
            int rectangleW = (int)(texture.Width * scale.X);
            int rectangleH = (int)(texture.Height * scale.Y);
            rectangle = new Rectangle(rectangleX, rectangleY, rectangleH, rectangleH);

            List<Rocket> rockets = GameManager.player.Rockets;

            for (int i = 0; i < rockets.Count; i++) {
                if (Helper.RectRect(rectangle, rockets[i].rectangle)) {
                    health--;
                    rockets[i].isDead = true;
                }
            }

            if (health < 0) {
                isDead = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
        
            for (int i = 0; i < baseEnemies.Count; i++) {
                baseEnemies[i].Draw(spriteBatch);
            }
        }

        private void Drop() {
            int randInt = rand.Next(100);

            if (randInt < baseEnemyDropChance) {
                BaseEnemy e = new BaseEnemy(position);
                e.SetAnimations();
                baseEnemies.Add(e);
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