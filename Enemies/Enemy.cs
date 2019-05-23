using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Spritesheet;
using Explore.Bullets;

namespace Explore.Enemies
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

        protected HealthBar healthBar;

        public abstract void SetAnimations();

        protected void CheckCollisions() {
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                Rectangle obs = platforms[i].Rectangle;
                
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

        protected void UpdateHealthBar() {
            healthBar.Update(new Vector2(position.X, position.Y - 50), Config.BaseEnemy["health"].IntValue, health);
        }

        protected bool IsOnPlatformEdge(int edgeSide) {
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                if (position.X > platforms[i].Rectangle.Right - edgeSide && position.X < platforms[i].Rectangle.Right) {
                    return true;
                } else if (position.X > platforms[i].Rectangle.Left && position.X < platforms[i].Rectangle.Left + edgeSide) {
                    return true;
                }
            }

            return false;
        }

        protected void CheckCollisionWithPlayerProjectiles() {

            foreach (var p in GameManager.player.Projectiles) {
                if (Helper.RectRect(rectangle, p.Rectangle)) {
                    if (p is Mine) {
                        health -= (p as Mine).Damage;
                        Effects.MineExplosion(position);
                    } else if (p is Bullet) {
                        health -= (p as Bullet).Damage;
                        ApplyKnockBack(new Vector2(-direction * speed * 1.5f, velocity.Y));
                        Effects.Explosion(position);
                    }

                    p.isDead = true;
                }
            }
        }

        protected void ApplyKnockBack(Vector2 knockBack) {
            velocity = knockBack;
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
}
