using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Spritesheet;
using Explore.Particles;

namespace Explore
{
    public class Player : GameObject
    {

        private int health;
        private int score;

        // Movement

        public Vector2 Position {
            get {
                return position;
            }
        }

        private int width = 32;
        private int height = 64;
        private int halfWidth;
        private int halfHeight;

        private int scaleFactor = 4;
        private int speed = 350;
        private Vector2 velocity;
        private int direction;
        private int jumpForce = -800;
        private int gravity = 30;

        private bool isGrounded = false;
        private bool jumping = false;

        // Animations

        private Spritesheet.Spritesheet spriteSheet;
        private Animation currentAnimation;
        private Animation idleAnim;
        private Animation runningAnimation;
        private Animation jumpingAnimation;
        private Animation idleAnimLeft;
        private Animation runningAnimationLeft;
        private Animation jumpingAnimationLeft;

        // Shooting

        private List<Bullet> bullets;
        public List<Bullet> Bullets {
            get {
                return bullets;
            }
        }

        private float initialShootingCooldown = 0.1f;
        private float shootingCooldown;

        private Texture2D gunTexture;
        private float gunAngle = 0;

        private Vector2 gunOrigin;
        private Vector2 gunPosition;
        private int gunScaleFactor = 3;
        private Vector2 gunScale;
        private int currentGunDirection = 1;

        private Vector2 gunShootPoint;
        private SpriteEffects gunSpriteEffect;

        private int ammo;

        // Rocket

        private float rocketInitialCooldown = 1f;
        private float rocketCooldown;

        private Texture2D rocketLauncherTexture;
        private Vector2 rocketLauncherScale;
        private Vector2 rocketLauncherShootPoint;

        private int currentRocketLauncherDirection = 1;

        private int rockets;

        public Player() : base("player") {
            rectangle = new Rectangle((int)position.X - halfWidth, (int)position.Y - halfHeight, width, height);
            halfWidth = width / 2;
            halfHeight = height / 2;

            bullets = new List<Bullet>();

            Reset();
        }

        // Create all animations from spritesheet

        public void SetAnimations() {
            texture = GameManager.Assets["square"];

            gunTexture = GameManager.Assets["gun2"];

            rocketLauncherTexture = GameManager.Assets["launcher"];

            Texture2D spriteSheetTexture = GameManager.Assets["mamba"];
            spriteSheet = new Spritesheet.Spritesheet(spriteSheetTexture).WithGrid((16, 16));

            idleAnim = spriteSheet.CreateAnimation((0, 1), (1, 1), (2, 1));
            runningAnimation = spriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0));
            jumpingAnimation = spriteSheet.CreateAnimation((0, 2), (1, 2), (2, 2));

            idleAnimLeft = idleAnim.FlipX();
            runningAnimationLeft = runningAnimation.FlipX();
            jumpingAnimationLeft = jumpingAnimation.FlipX();

            currentAnimation = idleAnim;

            idleAnim.Start(Repeat.Mode.Loop);
            runningAnimation.Start(Repeat.Mode.Loop);
            idleAnimLeft.Start(Repeat.Mode.Loop);
            runningAnimationLeft.Start(Repeat.Mode.Loop);
            currentAnimation.Start(Repeat.Mode.Loop);
        }

        public void Update() {
            if (Input.Respawn) {
                Reset();
            }
            PerformMovement();

            Vector2 cameraDesiredPosition = new Vector2(position.X, position.Y - 170);
            Game1.camera.Transform.Position = Vector2.SmoothStep(Game1.camera.Transform.Position, cameraDesiredPosition, 0.15f);

            UpdateHandGun();
            UpdateRocketLauncher();
            currentAnimation.Update(GameManager.gameTime);
        }

        private void PerformMovement() {
            isGrounded = false;
            // Check collision with all platforms in game

            List<Platform> platforms = GameManager.platforms;
            
            for (int i = 0; i < platforms.Count; i++) {

                Rectangle obs = platforms[i].rectangle;
                    
                string collision = Helper.RectangleCollision(rectangle, obs);

                if (collision != "false") {
                    switch (collision) {
                        case "top":
                            position.Y = obs.Bottom + halfHeight;
                            velocity.Y = gravity;
                            break;
                        case "bottom":
                            isGrounded = true;
                            position.Y = obs.Top - halfHeight;
                            break;
                        case "left":
                            position.X = obs.Left - halfWidth;
                            break;
                        case "right":
                            position.X = obs.Right + halfWidth;
                            break;
                    } 
                }
            }
        
            if (Input.Left) {
                direction = -1;
            } else if (Input.Right) {
                direction = 1;
            } else {
                direction = 0;
            }
            
            if (isGrounded && Input.Up) {
                velocity.Y = jumpForce;
                jumping = true;
            } else if (!isGrounded) {
                velocity.Y += gravity;
            } else if (isGrounded) {
                velocity.Y = 0;
                jumping = false;
            }

            velocity.X = direction * speed;

            position += velocity * GameManager.DeltaTime;

            rectangle = new Rectangle((int)position.X - halfWidth, (int)position.Y - halfHeight, width, height);
        }

        private void CheckForShooting() {
            if (Input.Space && shootingCooldown <= 0) {
                if (ammo > 0) {
                    Shoot();
                }
                shootingCooldown = initialShootingCooldown;
            } else {
                shootingCooldown -= GameManager.DeltaTime;
            }
        }

        private void Shoot() {
            Bullet b = new Bullet(gunShootPoint, currentGunDirection, "player");
            b.SetTexture(GameManager.Assets["bullet"]);
            bullets.Add(b);
            ammo--;
        }

        private void UpdateHandGun() {

            CheckForShooting();

            gunAngle = 0;
            gunOrigin = new Vector2(gunTexture.Width / 2, gunTexture.Height / 2);
            gunScale = new Vector2(gunScaleFactor, gunScaleFactor);

            int gunOffset = 45;
            int shootPointOffset = 80;

            if (direction == 1 && currentGunDirection != 1) {
                currentGunDirection = 1;
                gunSpriteEffect = SpriteEffects.None;
                gunOffset *= -1;
                shootPointOffset *= -1;
            } else if (direction == -1 && currentGunDirection != -1) {
                currentGunDirection = -1;
                gunSpriteEffect = SpriteEffects.FlipHorizontally;
                gunOffset *= -1;
                shootPointOffset *= -1;
            }

            gunPosition = new Vector2(position.X - gunOffset, position.Y);
            gunShootPoint = new Vector2(position.X - shootPointOffset, position.Y);

            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Update();

                if (bullets[i].isDead) {
                    bullets.RemoveAt(i);
                }
            }
        }

        private void UpdateRocketLauncher() {
            
            int offset = 30;

            Vector2 shootPointOffset = new Vector2(offset, rocketLauncherTexture.Height * rocketLauncherScale.Y);

            if (direction == 1 && currentRocketLauncherDirection != 1) {
                offset *= -1;
                currentRocketLauncherDirection = 1;
            }
        }

        public void Hit(int damage) {
            health -= damage;
        }

        public void ScoreUp(int amount) {
            score += amount;
        }

        public void Draw(SpriteBatch spriteBatch) {
            Vector2 scale = new Vector2(scaleFactor, scaleFactor);
            
            if (jumping) {
                
                if (currentAnimation == runningAnimation || currentAnimation == idleAnim) {
                    currentAnimation = jumpingAnimation;
                } else if (currentAnimation == runningAnimationLeft || currentAnimation == idleAnimLeft) {
                    currentAnimation = jumpingAnimationLeft;
                } else if (currentAnimation == jumpingAnimation && direction == -1) {
                    currentAnimation = jumpingAnimationLeft;
                } else if (currentAnimation == jumpingAnimationLeft && direction == 1) {
                    currentAnimation = jumpingAnimation;
                }

            } else {
                if (direction == 0) {
                    if (currentAnimation != idleAnim || currentAnimation != idleAnimLeft) {
                        if (currentAnimation == runningAnimation || currentAnimation == jumpingAnimation) {
                            currentAnimation = idleAnim;
                        } else if (currentAnimation == runningAnimationLeft || currentAnimation == jumpingAnimationLeft) {
                            currentAnimation = idleAnimLeft;
                        }
                    }
                } else if (direction == 1 && currentAnimation != runningAnimation) {
                    currentAnimation = runningAnimation;
                } else if (direction == -1 && currentAnimation != runningAnimationLeft) {
                    currentAnimation = runningAnimationLeft;
                }
            }

            // Debugging Rectangle
            //spriteBatch.Draw(texture, destinationRectangle: rectangle , color: Color.White);

            spriteBatch.Draw(currentAnimation, position, Color.White, 0, scale, 0);

            spriteBatch.Draw(gunTexture, gunPosition, null, Color.White, gunAngle, gunOrigin, gunScale, gunSpriteEffect, 0);
            
            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Draw(spriteBatch);
            }
        }

        public void DrawUI(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(GameManager.consolasFont, "Bullets: " + ammo.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(GameManager.consolasFont, "HP: " + health.ToString(), new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(GameManager.consolasFont, "Score: " + score.ToString(), new Vector2(10, 50), Color.White);
        }


        private void Reset() {
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            ammo = 100;
            health = 5;
            score = 0;
        }
    }
}