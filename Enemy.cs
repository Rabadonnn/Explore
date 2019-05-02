using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Spritesheet;

namespace Explore
{
    public class Enemy : GameObject
    {
        // Movement;

        private int width = 32;
        private int height = 48;
        private int health;
        private int healthPoints;
        private Vector2 velocity;
        private int gravity = 10;
        private int direction = 0;
        private bool isGrounded = false;
        private int speed = 200;

        //Shooting

        private bool shooting = false;
        private List<Bullet> bullets = new List<Bullet>();
        private float initialShootingCooldown = 0.5f;
        private float shootingCooldown;
        
        // Animations

        private Spritesheet.Spritesheet spriteSheet;
        private Animation currentAnimation;
        private Animation idleAnimation;
        private Animation idleLeftAnimation;
        private Animation runningAnimation;
        private Animation runningLeftAnimation;
        private Animation shootingAnimation;
        private Animation shootingLeftAnimation;

        public Enemy(Vector2 _position) : base("enemy") {
            position = _position;
            rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
            shootingCooldown = initialShootingCooldown;
        }

        public void SetAnimations() {
            texture = GameManager.Assets["guard"];
            spriteSheet = new Spritesheet.Spritesheet(texture).WithGrid((16, 16));

            idleAnimation = spriteSheet.CreateAnimation((0, 2), (1, 2));
            idleLeftAnimation = idleAnimation.FlipX();

            runningAnimation = spriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0));
            shootingAnimation = spriteSheet.CreateAnimation((0, 1), (1, 1), (2, 1), (3, 1));

            runningLeftAnimation = runningAnimation.FlipX();
            shootingLeftAnimation = shootingAnimation.FlipX();

            currentAnimation = idleAnimation;

            currentAnimation.Start(Repeat.Mode.Loop);
            runningAnimation.Start(Repeat.Mode.Loop);
            runningLeftAnimation.Start(Repeat.Mode.Loop);
            shootingAnimation.Start(Repeat.Mode.Loop);
            shootingLeftAnimation.Start(Repeat.Mode.Loop);
            idleLeftAnimation.Start(Repeat.Mode.Loop);
        }

        public void Update(Vector2 target) {

            isGrounded = false;

            // Collisions

            List<Platform> obstacles = GameManager.platforms;

            for (int i = 0; i < obstacles.Count; i++) {
                if (Collision.RectRect(rectangle, obstacles[i].rectangle)) {

                    Rectangle obs = obstacles[i].rectangle;
                    
                    // Best collision algorithm

                    float w = 0.5f * (rectangle.Width + obs.Width);
                    float h = 0.5f * (rectangle.Height + obs.Height);
                    float dx = rectangle.Center.X - obs.Center.X;
                    float dy = rectangle.Center.Y - obs.Center.Y;

                    if (Math.Abs(dx) <= w && Math.Abs(dy) <= h) {

                        // Collision
                        float wy = w * dy;
                        float hx = h * dx;

                        if (wy > hx) {
                            if (wy > -hx) {
                                // Collision on top
                                position.Y = obs.Bottom + width / 2;
                                velocity.Y = gravity;
                            } else {
                                // On left
                                position.X = obs.Left - width / 2;
                            }
                        } else {
                            if (wy > -hx) {
                                // on right
                                position.X = obs.Right + width / 2;
                            } else {
                                // on bottom
                                isGrounded = true;
                                position.Y = obs.Top - height/ 2;
                            }
                        }
                    }
                }
            }

            if (isGrounded) {
                velocity.Y = 0;
            } else {
                velocity.Y += gravity;
            }

            if (isGrounded) {
                if (target.X > position.X) {
                    direction = 1;
                } else {
                    direction = -1;
                }
            } else {
                direction = 0;
            }

            CheckForShooting();

            velocity.X = direction * speed;

            position += velocity *  GameManager.DeltaTime;
            rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);


            currentAnimation.Update(GameManager.gameTime);

            CheckForDead();
        }

        private void CheckForShooting() {
            if (direction != 0) {
                if (shootingCooldown <= 0) {
                    
                    Bullet bulletToShoot = new Bullet(position, direction);
                    bulletToShoot.SetTexture(GameManager.Assets["bullet"]);
                    bullets.Add(bulletToShoot);

                    shooting = true;
                    shootingCooldown = initialShootingCooldown;
                } else {
                    shooting = false;
                    shootingCooldown -= GameManager.DeltaTime;
                }
            }
            

            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Update();

                if (bullets[i].isDead) {
                    bullets.RemoveAt(i);
                }
            }
        }

        private void CheckForDead() {
            if (position.X > GameManager.Width || position.X < -GameManager.Width || position.Y > GameManager.Height) {
                isDead = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {

            if (shooting) {
                if (direction == 1) {
                    currentAnimation = shootingAnimation;
                } else if (direction == -1) {
                    currentAnimation = shootingLeftAnimation;
                }
            } else if (direction == 0 && (currentAnimation != idleAnimation || currentAnimation != idleLeftAnimation)) {
                if (currentAnimation == runningAnimation) {
                    currentAnimation = idleAnimation;
                } else if (currentAnimation == runningLeftAnimation) {
                    currentAnimation = idleLeftAnimation;
                }
            } else if (direction == 1 && currentAnimation != runningAnimation) {
                currentAnimation = runningAnimation;
            } else if (direction == -1 && currentAnimation != runningLeftAnimation) {
                currentAnimation = runningLeftAnimation;
            }

            // Texture2D square = GameManager.Assets["square"];
            // spriteBatch.Draw(square, destinationRectangle: rectangle);

            Vector2 scale = new Vector2(3, 3);
            spriteBatch.Draw(currentAnimation, position, Color.White, 0, scale, 0);

            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Draw(spriteBatch);
            }

        }
    }
}