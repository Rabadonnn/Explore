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

        // Building

        public List<Platform> platforms;
        private float initialBuildingCooldown = 0.17f;
        private float buildingCooldown;

        // FX

        private Settings walkEffectSettings;
        private ParticleSystem walkEffect;


        public Player() : base("player") {
            rectangle = new Rectangle((int)position.X - halfWidth, (int)position.Y - halfHeight, width, height);
            platforms = new List<Platform>();
            halfWidth = width / 2;
            halfHeight = height / 2;

            bullets = new List<Bullet>();

            SetFX();
        }

        // Create all animations from spritesheet

        public void SetAnimations() {
            texture = GameManager.Assets["square"];

            gunTexture = GameManager.Assets["gun2"];

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

            walkEffect.SetTexture(GameManager.Assets["square"]);
        }

        public void Update() {
            if (Input.Respawn) {
                Reset();
            }
            PerformMovement();

            Vector2 cameraDesiredPosition = new Vector2(position.X, position.Y - 170);
            Game1.camera.Transform.Position = Vector2.SmoothStep(Game1.camera.Transform.Position, cameraDesiredPosition, 0.15f);


            if (IsBewteenBounds()) {
                CheckForBuilding();
            }

            UpdateGun();

            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].Update();

                if (platforms[i].isDead) {
                    platforms.RemoveAt(i);
                }
            }

            currentAnimation.Update(GameManager.gameTime);
            UpdateFX();
        }
        private void KeepBetweenBounds() {
            if (position.X > GameManager.ScreenWidth) {
                position.X = GameManager.ScreenWidth;
            } else if (position.X < -GameManager.ScreenWidth) {
                position.X = -GameManager.ScreenWidth;
            } else if (position.Y < -GameManager.ScreenHeight / 2) {
                position.Y = -GameManager.ScreenHeight / 2;
            } else if (position.Y > GameManager.ScreenHeight) {
                position.Y = GameManager.ScreenHeight;
            }
        }

        private bool IsBewteenBounds() {
            return position.X < GameManager.ScreenWidth &&
                    position.X > -GameManager.ScreenWidth &&
                    position.Y > -GameManager.ScreenHeight / 2 &&
                    position.Y < GameManager.ScreenHeight;
        }

        private void PerformMovement() {
            isGrounded = false;

            platforms.AddRange(GameManager.platforms);

            // Check collision with all platforms in game
            
            for (int i = 0; i < platforms.Count; i++) {
                if (Collision.RectRect(rectangle, platforms[i].rectangle)) {

                    Rectangle obs = platforms[i].rectangle;
                    
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
                                position.Y = obs.Bottom + halfHeight;
                                velocity.Y = gravity;
                            } else {
                                // On left
                                position.X = obs.Left - halfWidth;
                            }
                        } else {
                            if (wy > -hx) {
                                // on right
                                position.X = obs.Right + halfWidth;
                            } else {
                                // on bottom
                                isGrounded = true;
                                position.Y = obs.Top - halfHeight;
                            }
                        }
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
                Shoot();
                shootingCooldown = initialShootingCooldown;
            } else {
                shootingCooldown -= GameManager.DeltaTime;
            }
        }

        private void Shoot() {
            Bullet b = new Bullet(gunShootPoint, currentGunDirection);
            b.SetTexture(GameManager.Assets["bullet"]);
            bullets.Add(b);
        }

        private void UpdateGun() {

            CheckForShooting();

            gunAngle = 0;
            gunOrigin = new Vector2(gunTexture.Width / 2, gunTexture.Height / 2);
            gunScale = new Vector2(gunScaleFactor, gunScaleFactor);

            int gunOffset = 30;
            int shootPointOffset = 65;

            if (direction == 1 && currentGunDirection != 1) {
                currentGunDirection = 1;
                gunSpriteEffect = SpriteEffects.None;
            } else if (direction == -1 && currentGunDirection != -1) {
                currentGunDirection = -1;
                gunSpriteEffect = SpriteEffects.FlipHorizontally;
            }

            if (currentGunDirection == 1) {
                gunPosition = new Vector2(position.X + gunOffset, position.Y);
                gunShootPoint = new Vector2(position.X + shootPointOffset, position.Y);
            } else if (currentGunDirection == -1) {
                gunPosition = new Vector2(position.X - gunOffset, position.Y);
                gunShootPoint = new Vector2(position.X - shootPointOffset, position.Y);
            }

            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Update();

                if (bullets[i].isDead) {
                    bullets.RemoveAt(i);
                }
            }
        }

        private void CheckForBuilding() {
            if (Input.RightClick && !isGrounded && buildingCooldown <= 0) {
                Platform platformToBuild = new Platform(new Vector2(position.X, position.Y + rectangle.Height), new Vector2(64, 32));
                platformToBuild.SetTexture(GameManager.Assets["platform3"]);
                platforms.Add(platformToBuild);
                buildingCooldown = initialBuildingCooldown;
            } else {
                buildingCooldown -= GameManager.DeltaTime;
            }       
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
                } else if (direction == 1) {
                    if (currentAnimation != runningAnimation) {
                        currentAnimation = runningAnimation;
                    }
                } else if (direction == -1) {
                    if (currentAnimation != runningAnimationLeft) {
                        currentAnimation = runningAnimationLeft;
                    }
                }
            }

            // Debugging Rectangle
            //spriteBatch.Draw(texture, destinationRectangle: rectangle , color: Color.White);

            spriteBatch.Draw(currentAnimation, position, Color.White, 0, scale, 0);

            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].Draw(spriteBatch);
            }

            spriteBatch.Draw(gunTexture, gunPosition, null, Color.White, gunAngle, gunOrigin, gunScale, gunSpriteEffect, 0);
            
            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Draw(spriteBatch);
            }

            DrawFX(spriteBatch);
        }

        private void SetFX() {
            walkEffectSettings = new Settings() {
                number_per_frame = 3,
                lifespan = 0.35f,
                accX = new Vector2(28, 95),
                accY = new Vector2(-35, -6),
                velocity = new Vector2(88, 29),
                speed = 500
            };
            walkEffect = new ParticleSystem(walkEffectSettings, new Rectangle(rectangle.Left, rectangle.Bottom, 1, 1));
            walkEffect.enabled = false;
        }

        private void UpdateFX() {
            walkEffect.Update();
            if (direction == 1) {
                walkEffect.rectangle = new Rectangle(rectangle.Left, rectangle.Bottom, 1, 1);
                if (walkEffect.settings.accX.X > 0 && walkEffect.settings.accY.X > 0) {
                    walkEffect.settings.accX *= -1;
                    walkEffect.settings.accY *= -1;
                    walkEffect.settings.velocity *= -1;
                }
            }
            else if (direction == -1) {
                walkEffect.rectangle = new Rectangle(rectangle.Right, rectangle.Bottom, 1, 1);
                if (walkEffect.settings.accX.X < 0 && walkEffect.settings.accY.X < 0) {
                    walkEffect.settings.accX *= -1;
                    walkEffect.settings.accY *= -1;
                    walkEffect.settings.velocity *= -1;
                }
            } 
        }

        private void DrawFX(SpriteBatch spriteBatch) {
            walkEffect.Draw(spriteBatch);
        }


        private void Reset() {
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            shootingCooldown = initialShootingCooldown;
            buildingCooldown = initialBuildingCooldown;
        }
    }
}