using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Explore
{
    public class DropShip : GameObject
    {
        private int size = 64;
        private Random rand;
        private Vector2 checkPoint;

        private int enemyDropChance = 50;
        private int nukeDropChance = 25;

        private Nuke nuke;

        public DropShip(Vector2 _position) : base("dropship") {
            position = _position;
            rectangle = new Rectangle((int)(position.X - size / 2), (int)(position.Y - size / 2), size, size);
            rand = new Random();
            checkPoint = MakeNewCheckPoint();
        }

        public void Update() {
            position = Vector2.Lerp(position, checkPoint, 0.01f);

            if (Collision.Distance(position, checkPoint) < 50) {
                checkPoint = MakeNewCheckPoint();
                CheckForDrop();
                //DropBadGuys(1);
            }

            if (nuke != null) {
                nuke.Update();

                if (nuke.isDead) {
                    nuke = null;
                }
            }

            rectangle = new Rectangle((int)(position.X - size / 2), (int)(position.Y - size / 2), size, size);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);

            if (nuke != null) {
                nuke.Draw(spriteBatch);
            }
        }

        private Vector2 MakeNewCheckPoint() {
            return new Vector2(rand.Next(-GameManager.Width / 2, GameManager.Width / 2), rand.Next(-GameManager.Height / 2, 0));
        }

        private void CheckForDrop() {
            int randInt = rand.Next(100);
            
            if (randInt < enemyDropChance) {
                if (randInt < nukeDropChance) {
                    DropNuke();
                } else {
                    DropBadGuys(1);
                }
            }
        }

        private void DropBadGuys(int amount) {
            for (int i = 0; i < amount; i++) {
                Enemy enemyToDrop = new Enemy(position);
                enemyToDrop.SetAnimations();
                GameManager.enemies.Add(enemyToDrop);
            }
        }

        private void DropNuke() {
            if (nuke == null) {
                nuke = new Nuke(position);
                nuke.SetTexture(GameManager.Assets["nuke"]);
            }
        }
    }
}