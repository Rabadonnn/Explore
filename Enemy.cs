using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Spritesheet;

namespace Explore
{
    public abstract class Enemy : GameObject
    {

        protected int health;
        protected int scoreValue;

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

        protected void CheckCollisions() {
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                Rectangle obs = platforms[i].rectangle;
                
                if (Helper.RectRectExtended(rectangle, obs) != Helper.Collision.NoCollision) {
                    switch (Helper.RectRectExtended(rectangle, obs)) {
                        case Helper.Collision.Top:
                            position.Y = obs.Bottom + height / 2;
                            velocity.Y = 10 * gravity;
                            break;
                        case Helper.Collision.Bottom:
                            isGrounded = true;
                            position.Y = obs.Top - height / 2;
                            break;
                        case Helper.Collision.Left:
                            position.X = obs.Left - width / 2;
                            break;
                        case Helper.Collision.Right:
                            position.X = obs.Right + width / 2;
                            break;
                    } 
                }
            }
        }

        protected bool IsOnPlatformEdge(int edgeSide) {
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                if (position.X > platforms[i].rectangle.Right - edgeSide && position.X < platforms[i].rectangle.Right) {
                    return true;
                } else if (position.X > platforms[i].rectangle.Left && position.X < platforms[i].rectangle.Left + edgeSide) {
                    return true;
                }
            }

            return false;
        }

        protected bool CollisionWithPlayerBullet() {

            for (int i = 0; i < GameManager.player.Bullets.Count; i++) {
                
                if (Helper.RectRect(rectangle, GameManager.player.Bullets[i].rectangle)) {
                    GameManager.player.Bullets[i].isDead = true;
                    Explosions.Explosion(position);
                    return true;
                }
            }
            return false;
        }

        protected void ApplyKnockBack(Vector2 knockBack) {
            velocity = knockBack;
        }

        protected bool CollisionWithMine() {
            List<Mine> mines = GameManager.player.Mines;
            for (int i = 0; i < mines.Count; i++) {
                if (Helper.RectRect(rectangle, mines[i].rectangle) && mines[i].IsPlaced ) {
                    mines[i].Explode();
                    return true;
                } else if (Helper.Distance(new Vector2(mines[i].rectangle.Center.X, mines[i].rectangle.Center.Y), position) < 75 && mines[i].exploded) {
                    return true;
                }
            }
            return false;
        }
        protected void UpdateBullets() {
            for (int i = 0; i < bullets.Count; i++) {
                if (bullets[i].isDead) {
                    bullets.RemoveAt(i);
                } else {
                    bullets[i].Update();
                }
            }
        }

        protected void DrawBullets(SpriteBatch spriteBatch) {
            for (int i = 0; i < bullets.Count; i++) {
                bullets[i].Draw(spriteBatch);
            }
        }
    }

    //TODO: More enemies


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

        private int previousHealth;

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

            if (position.Y > GameManager.ScreenHeight || health < 0) {
                isDead = true;
                GameManager.player.KilledEnemy();
            } 

            CheckForShooting();

            UpdateBullets();

            if (CollisionWithPlayerBullet()) {
                health -= GameManager.player.Bullets[0].Damage;
                ApplyKnockBack(new Vector2(-direction * speed * 1.5f, velocity.Y));
            }

            if (CollisionWithMine()) {
                health -= GameManager.player.Mines[0].Damage;
            }

            currentAnimation.Update(GameManager.gameTime);

            for (int i = 0; i < bullets.Count; i++) {
                if (Helper.RectRect(GameManager.player.rectangle, bullets[i].rectangle)) {
                    GameManager.player.Hit(damage);
                    bullets[i].isDead = true;
                }
            }
        }

        private void CheckForShooting() {
            if (shootingCooldown <= 0 && isGrounded && Helper.Distance(target, position) < range) {
                
                Bullet b = new Bullet(position, direction, "enemy");
                b.SetTexture(GameManager.Assets["bullet"]);
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
        }
    }
}