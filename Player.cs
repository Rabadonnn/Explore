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
        
        #region Variables

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

        private enum Gun {
            Launcher,
            HandGun
        };

        private Gun currentGun = Gun.HandGun;

        private int currentGunDirection = 1;

        // HandGun

        private List<Bullet> bullets;
        public List<Bullet> Bullets {
            get {
                return bullets;
            }
        }

        private float handGunInitialShootingCooldown = 0.1f;
        private float handGunShootingCooldown;

        private Texture2D gunTexture;
        private float gunAngle = 0;

        private Vector2 gunOrigin;
        private Vector2 gunPosition;
        private int gunScaleFactor = 3;
        private Vector2 gunScale;

        private Vector2 gunShootPoint;
        private SpriteEffects gunSpriteEffect;

        private int handGunAmmo;

        // Rocket

        private List<Rocket> rockets;
        public List<Rocket> Rockets {
            get {
                return rockets;
            }
        }

        private Vector2 rocketLauncherPosition;

        private float rocketInitialCooldown = 1f;
        private float rocketCooldown;

        private Texture2D rocketLauncherTexture;
        private Vector2 rocketLauncherScale;
        private Vector2 rocketLauncherShootPoint;

        private int rocketsCount;

        // Mines

        private List<Mine> mines;
        public List<Mine> Mines {
            get {
                return mines;
            }
        }

        private float initialMineCooldown = 2f;
        private float mineCooldown;

        private int mineCount;

        #endregion

        public Player() : base("player") {
            Reset();
        }

        // Create all animations from spritesheet

        #region Animation

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

        #endregion

        public void Update() {
            if (Input.Respawn || health < 0) {
                Reset();
            }

            PerformMovement();

            UpdateGuns();

            Vector2 cameraDesiredPosition = new Vector2(position.X, position.Y - 170);
            Game1.camera.Transform.Position = Vector2.SmoothStep(Game1.camera.Transform.Position, cameraDesiredPosition, 0.15f);

            currentAnimation.Update(GameManager.gameTime);
        }

        #region Movement

        private void PerformMovement() {
            isGrounded = false;
            // Check collision with all platforms in game

            List<Platform> platforms = GameManager.platforms;
            
            for (int i = 0; i < platforms.Count; i++) {

                Rectangle obs = platforms[i].rectangle;

                if (Helper.RectangleCollision(rectangle, obs) != Helper.Collision.NoCollision) {

                    
                    Point leftCorner = new Point(rectangle.Bottom, rectangle.Left);
                    Point rightCorner = new Point(rectangle.Bottom, rectangle.Right);

                    switch (Helper.RectangleCollision(rectangle, obs)) {
                        case Helper.Collision.Top:
                            position.Y = obs.Bottom + halfHeight;
                            velocity.Y = gravity;
                            break;
                        case Helper.Collision.Bottom:
                            isGrounded = true;
                            position.Y = obs.Top - halfHeight;
                            break;
                        case Helper.Collision.Left:
                            position.X = obs.Left - halfWidth;
                            break;
                        case Helper.Collision.Right:
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

        #endregion

        #region Shooting

        private void UpdateGuns() {

            CheckForShooting();

            if (Input.E && currentGun != Gun.HandGun) {
                currentGun = Gun.HandGun;
            } else if (Input.Q && currentGun != Gun.Launcher) {
                currentGun = Gun.Launcher;
            }

            if (currentGun == Gun.HandGun) {
                UpdateHandGun();
            } else if (currentGun == Gun.Launcher) {
                UpdateRocketLauncher();
            }

            for (int i = 0; i < bullets.Count; i++) {
                if (bullets[i].isDead) {
                    bullets.RemoveAt(i);
                } else {
                    bullets[i].Update();
                }
            }

            for (int i = 0; i < rockets.Count; i++) {
                if (rockets[i].isDead) {
                    rockets.RemoveAt(i);
                } else {
                    rockets[i].Update();
                }
            }

            for (int i = 0; i < mines.Count; i++) {
                if (mines[i].isDead) {
                    mines.RemoveAt(i);
                } else {
                    mines[i].Update();
                }
            }

        }


        private void CheckForShooting() {
            if (Input.Space && currentGun == Gun.HandGun && handGunShootingCooldown <= 0 && handGunAmmo > 0) {
                ShootHandGun();
                handGunShootingCooldown = handGunInitialShootingCooldown;
            } else {
                handGunShootingCooldown -= GameManager.DeltaTime;
            }

            if (Input.Space && currentGun == Gun.Launcher && rocketCooldown <= 0 && rocketsCount > 0) {
                LaunchRocket();
                rocketCooldown = rocketInitialCooldown;
            } else {
                rocketCooldown -= GameManager.DeltaTime;
            }

            if (Input.V && mineCooldown <= 0 && mineCount > 0) {
                ThrowMine();
                mineCooldown = initialMineCooldown;
            } else {
                mineCooldown -= GameManager.DeltaTime;
            }
        }

        private void ShootHandGun() {
            Bullet b = new Bullet(gunShootPoint, currentGunDirection, "player");
            b.SetTexture(GameManager.Assets["bullet"]);
            bullets.Add(b);
            handGunAmmo--;
        }

        private void LaunchRocket() {
            Rocket r = new Rocket(rocketLauncherShootPoint, "player");
            r.SetTexture(GameManager.Assets["rpg_ammo"]);
            rockets.Add(r);
            rocketsCount--;
        }

        private void ThrowMine() {
            Mine m = new Mine(new Vector2(position.X, rectangle.Top), currentGunDirection);
            m.SetAnimations();
            mines.Add(m);
            mineCount--;
        }

        private void UpdateHandGun() {
            gunAngle = 0;
            gunOrigin = new Vector2(gunTexture.Width / 2, gunTexture.Height / 2);
            gunScale = new Vector2(gunScaleFactor, gunScaleFactor);

            int gunOffset = 45;
            int shootPointOffset = 80;

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
        }

        private void UpdateRocketLauncher() {
            
            int offset = 30;

            rocketLauncherScale = new Vector2(1.4f, 1.4f);

            if (direction == 1 && currentGunDirection != 1) {
                currentGunDirection = 1;
            } else if (direction == -1 && currentGunDirection != -1) {
                currentGunDirection = -1;
            }

            if (currentGunDirection == 1) {
                rocketLauncherPosition = new Vector2(position.X + offset, position.Y);
                rocketLauncherShootPoint = new Vector2(position.X + offset, position.Y + rocketLauncherTexture.Height / 2);
            } else if (currentGunDirection == -1) {
                rocketLauncherPosition = new Vector2(position.X - offset, position.Y);
                rocketLauncherShootPoint = new Vector2(position.X - offset, position.Y + rocketLauncherTexture.Height / 2);
            }
        }


        #endregion

        public void Hit(int damage) {
            health -= damage;
        }

        public void ScoreUp(int amount) {
            score += amount;
        }

        public void GiveHealth(int amount) {
            health += amount;
        }

        public void GiveHandGunAmmo(int amount) {
            handGunAmmo += amount;
        }

        #region  Draw

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

            if (currentGun == Gun.HandGun) {
                spriteBatch.Draw(gunTexture, gunPosition, null, Color.White, gunAngle, gunOrigin, gunScale, gunSpriteEffect, 0);
            } else if (currentGun == Gun.Launcher) {
                spriteBatch.Draw(rocketLauncherTexture, rocketLauncherPosition, null, Color.White, 0, new Vector2(rocketLauncherTexture.Width / 2, rocketLauncherTexture.Height / 2), rocketLauncherScale, SpriteEffects.None, 0);
            }
      
            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Draw(spriteBatch);
            }

            for (int i = 0; i < rockets.Count; i++) {
                rockets[i].Draw(spriteBatch);
            }

            for (int i = 0; i < mines.Count; i++) {
                mines[i].Draw(spriteBatch);
            }
        }

        public void DrawUI(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(GameManager.consolasFont, "Bullets: " + handGunAmmo.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(GameManager.consolasFont, "Rockets: " + rocketsCount.ToString(), new Vector2(10, 30), Color.White);
            spriteBatch.DrawString(GameManager.consolasFont, "Mines: " + mineCount.ToString(), new Vector2(10, 50), Color.White);
            spriteBatch.DrawString(GameManager.consolasFont, "HP: " + health.ToString(), new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(GameManager.consolasFont, "Score: " + score.ToString(), new Vector2(10, 90), Color.White);
        }

        #endregion

        private void Reset() {
            rectangle = new Rectangle((int)position.X - halfWidth, (int)position.Y - halfHeight, width, height);
            halfWidth = width / 2;
            halfHeight = height / 2;

            bullets = new List<Bullet>();

            rockets = new List<Rocket>();

            mines = new List<Mine>();

            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            handGunAmmo = 100;
            health = 5;
            score = 0;
            rocketsCount = 10;
            mineCount = 5;
        }
    }
}