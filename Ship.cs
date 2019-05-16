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

        protected int baseEnemyDropChance = 50;

        public BaseShip(Vector2 _position) {

            health = 10;

            position = _position;
            rand = new Random();
            checkPoint = MakeNewCheckPoint();
        }

        public virtual void SetTexture() {
            texture = GameManager.Assets["dropship"];
        }

        public override void Update() {
            if (Helper.Distance(position, checkPoint) < 25) {
                checkPoint = MakeNewCheckPoint();

                if (IsAbovePlatform()) {
                    Drop();
                }

            } else {
                position = Vector2.Lerp(position, checkPoint, 0.01f);
            }

            UpdateRectangle();

            CheckCollisions();

            CheckLife();
        }

        protected void CheckLife() {
            if (health < 1) {
                isDead = true;
                GameManager.player.DestroyedShip();
            }
        }
        

        protected void CheckCollisions() {
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
                    if (position.X > platforms[i].rectangle.Left &&
                        position.X < platforms[i].rectangle.Right && 
                        position.Y < platforms[i].rectangle.Top) {
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

    public class BombShip : BaseShip {
        
        private float initialDropCooldown = 5f;
        private float dropCooldown = 0;
        private int width = 62;
        private int height = 48;

        private List<Bomb> bombs;

        private float lifetime;

        public BombShip(Vector2 _position) : base(_position) {
            health = 2;
            position = _position;
            rand = new Random();
            bombs = new List<Bomb>();
            checkPoint = MakeNewCheckPoint();
            lifetime = rand.Next(5, 10);
        }

        public override void SetTexture() {
            texture = GameManager.Assets["bombship"];
        }

        public override void Update() {
            if (!isDead) {

                if (Helper.Distance(position, checkPoint) < 30) {
                    checkPoint = new Vector2(GameManager.player.Position.X, rand.Next(-GameManager.ScreenHeight, 0));
                }

                if (lifetime <= 0) {
                    checkPoint = new Vector2(rand.Next(GameManager.LeftBound, GameManager.RightBound), -500);
                }

                position = Vector2.Lerp(position, checkPoint, 0.02f);

                if (dropCooldown <= 0) {
                    if (Helper.Distance(position, GameManager.player.Position) < 70) {
                        DropBomb();
                        dropCooldown = initialDropCooldown;
                    }
                } else {
                    dropCooldown -= GameManager.DeltaTime;
                }

                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
            
                CheckCollisions();
                CheckLife();

                if (position.Y < -500) {
                    isDead = true;
                }
            }

            for (int i = 0; i < bombs.Count; i++) {
                if (bombs[i].isDead) {
                    bombs.RemoveAt(i);
                } else {
                    bombs[i].Update();
                    if (Helper.RectRect(GameManager.player.rectangle, bombs[i].rectangle)) {
                        GameManager.player.Hit(rand.Next(2, 4));
                        bombs[i].isDead = true;
                    }
                }
            }
        }

        private void DropBomb() {
            Bomb b = new Bomb(position);
            b.SetTexture();
            bombs.Add(b);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        
            for (int i = 0; i < bombs.Count; i++) {
                bombs[i].Draw(spriteBatch);
            }
        }
    }
}