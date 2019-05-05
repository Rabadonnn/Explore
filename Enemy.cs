using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Spritesheet;

namespace Explore
{
    public abstract class Enemy : GameObject
    {
        // Movement

        protected int width;
        protected int height;
        protected int speed;
        protected int direction;
        protected Vector2 velocity;
        protected int gravity;
        protected bool isGrounded;

        // Shooting

        protected List<Bullet> bullets;
        protected float initialShootingCooldown;
        protected float shootingCooldown;

        public abstract void SetAnimations();

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }


    public class BaseEnemy : Enemy
    {

        public BaseEnemy(Vector2 _position) {
            position = _position;
            rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);
        }

        public override void SetAnimations() {

        }

        public override void Update() {
            rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                Rectangle obs = platforms[i].rectangle;
                
                string collision = Helper.RectangleCollision(rectangle, obs);

                if (collision != "false") {
                    switch (collision) {
                        case "top":
                            position.Y = obs.Bottom + height / 2;
                            velocity.Y = gravity;
                            break;
                        case "bottom":
                            isGrounded = true;
                            position.Y = obs.Top - height / 2;
                            break;
                        case "left":
                            position.X = obs.Left - width / 2;
                            break;
                        case "right":
                            position.X = obs.Right + width / 2;
                            break;
                    } 
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {

        }
    }
}