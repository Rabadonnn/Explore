using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Explore
{
    public class DropShip : GameObject
    {
        private int width = 64;
        private int height = 64;
        private Vector2 velocity;
        private float speed = 200;
        private Random rand = new Random();
        private int upperBorder = -100;

        private Vector2 checkPoint;
        private int dropChance = 20;
        private bool firstDrop = true;

        public DropShip(Vector2 _position) : base("dropship") {
            position = _position;
            rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);

            checkPoint = MakeNewCheckPoint();
        }

        public void Update() {
            velocity = checkPoint - position;
            velocity.Normalize();
            position += velocity * speed * GameManager.DeltaTime;

            bool canDrop = false;

            if (Collision.Distance(position, checkPoint) < 10) {
                if (rand.Next(100) < dropChance) {

                    List<Platform> availablePlatforms = GameManager.platforms;
                    availablePlatforms.AddRange(GameManager.player.platforms);

                    for (int i = 0; i < availablePlatforms.Count; i++) {
                        if (position.X < availablePlatforms[i].rectangle.Right && position.X > availablePlatforms[i].rectangle.Left) {
                            canDrop = true;
                            break;
                        }
                    }

                    if (canDrop) {
                        DropBadGuys(1);
                    }

                    checkPoint = MakeNewCheckPoint();
                } else if (firstDrop){
                    List<Platform> availablePlatforms = GameManager.platforms;
                    availablePlatforms.AddRange(GameManager.player.platforms);

                    for (int i = 0; i < availablePlatforms.Count; i++) {
                        if (position.X < availablePlatforms[i].rectangle.Right && position.X > availablePlatforms[i].rectangle.Left) {
                            canDrop = true;
                            break;
                        }
                    }

                    if (canDrop) {
                        DropBadGuys(1);
                    }
                    firstDrop = false;
                    checkPoint = MakeNewCheckPoint();
                    
                } else {
                    checkPoint = MakeNewCheckPoint();
                }
            } 

            rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        private Vector2 MakeNewCheckPoint() {
            return new Vector2(rand.Next(-GameManager.Width / 2, GameManager.Width / 2), rand.Next(-GameManager.Height / 2, 0));
        }

        private void DropBadGuys(int number) {
            for (int i = 0; i < number; i++) {
                Enemy enemyToDrop = new Enemy(position);
                enemyToDrop.SetAnimations();
                GameManager.enemies.Add(enemyToDrop);
            }
        }
    }
}