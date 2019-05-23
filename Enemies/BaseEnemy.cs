using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spritesheet;
using System.Collections.Generic;
using Explore.Bullets;

namespace Explore.Enemies
{
    public class BaseEnemy : Enemy
    {
        private int damage;
        private int range;

        private Vector2 target;

        private Spritesheet.Spritesheet spriteSheet;

        private Animation currentAnimation;

        private Animation idleAnim;
        private Animation idleAnim_Left;
        private Animation runAnim;
        private Animation runAnim_Left;

        private Vector2 scale = new Vector2(3, 3);

        private int jumpForce;

        public BaseEnemy(Vector2 _position){
            position = _position;
            rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

            width = 32;
            height = 48;

            health = Config.BaseEnemy["health"].IntValue;

            damage = Config.BaseEnemy["damage"].IntValue;
            range = Config.BaseEnemy["range"].IntValue;

            speed = Config.BaseEnemy["speed"].IntValue;
            direction = 0;
            gravity = Config.BaseEnemy["gravity"].IntValue;
            jumpForce = Config.BaseEnemy["jumpforce"].IntValue;

            bullets = new List<Bullet>();

            initialShootingCooldown = Config.BaseEnemy["shootingCooldown"].FloatValue;
            shootingCooldown = initialShootingCooldown;

            healthBar = new HealthBar();
        }

        public override void SetAnimations() {
            
            spriteSheet = new Spritesheet.Spritesheet(GameManager.Assets["guard"]).WithGrid((16, 16));

            idleAnim = spriteSheet.CreateAnimation((0, 0), (1, 0));
            idleAnim_Left = idleAnim.FlipX();
            runAnim = spriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0));
            runAnim_Left = runAnim.FlipX();

            currentAnimation = idleAnim;

            idleAnim.Start(Repeat.Mode.Loop);
            idleAnim_Left.Start(Repeat.Mode.Loop);
            runAnim.Start(Repeat.Mode.Loop);
            runAnim_Left.Start(Repeat.Mode.Loop);
            currentAnimation.Start(Repeat.Mode.Loop);

            healthBar.SetTexture();
        }

        public override void Update() {

            isGrounded = false;

            rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

            CheckCollisions();

            target = GameManager.player.Position;

            if (Helper.Distance(position, target) > 35) {
            
                if (isGrounded && IsOnPlatformEdge(20)) {
                    velocity.Y = jumpForce;
                } else if (!isGrounded) {
                    velocity.Y += gravity;
                } else if (isGrounded) {
                    velocity.Y = 0;
                }

                if (isGrounded) {
                    if (target.X > position.X) {
                        velocity.X = speed;
                        direction = 1;
                    } else if (target.X < position.X) {
                        velocity.X = -speed;
                        direction = -1;
                    }
                }
            }
            
            position += velocity * GameManager.DeltaTime;

            if (position.Y > GameManager.ScreenHeight || health < 1) {
                isDead = true;
                GameManager.player.KilledEnemy();
            } 

            CheckCollisionWithPlayerProjectiles();

            CheckForShooting();

            UpdateBullets();

            UpdateHealthBar();

            currentAnimation.Update(GameManager.gameTime);

            for (int i = 0; i < bullets.Count; i++) {
                if (Helper.RectRect(GameManager.player.Rectangle, bullets[i].Rectangle)) {
                    GameManager.player.Hit(damage);
                    bullets[i].isDead = true;
                }
            }
        }

        private void CheckForShooting() {
            if (shootingCooldown <= 0 && isGrounded && Helper.Distance(target, position) < range) {
                
                Bullet b = new Bullet(position, direction);
                b.SetTexture();
                bullets.Add(b);

                shootingCooldown = initialShootingCooldown;
            } else {
                shootingCooldown -= GameManager.DeltaTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            //spriteBatch.Draw(texture, rectangle, Color.White);
            
            if (direction == 1 && currentAnimation != runAnim) {
                currentAnimation = runAnim;
            } else if (direction == -1 && currentAnimation != runAnim_Left) {
                currentAnimation = runAnim_Left;
            }

            spriteBatch.Draw(currentAnimation, position, Color.White, 0, scale, 0);

            DrawBullets(spriteBatch);

            healthBar.Draw(spriteBatch);
        }
    }
}