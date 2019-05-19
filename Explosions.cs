using Explore.Particles;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Explore
{
    public static class Explosions
    {
        private static List<ParticleSystem> explosions = new List<ParticleSystem>();

        public static void Update() {
            for (int i = 0; i < explosions.Count; i++) {
                if (explosions[i].enabled) {
                    explosions[i].Update();
                } else {
                    explosions.RemoveAt(i);
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < explosions.Count; i++) {
                explosions[i].Draw(spriteBatch);
            }
        }

        private static Vector2 baseAcc = new Vector2(-7, 7);
        private static List<Color> colors = new List<Color>() { Color.Red, Color.Orange, Color.Yellow };

        public static void Explosion(Vector2 position) {
            Settings settings = new Settings() {
                accX = baseAcc,
                accY = baseAcc,
                color = colors,
                size = new Point(3, 7),
                number_per_frame = 6,
                lifespan = 0.06f,
                speed = 700,
                oneTime = true
            };
            AddFX(settings, position);
        }

        public static void BigExplosion(Vector2 position) {
            Settings settings = new Settings() {
                accX = baseAcc,
                accY = baseAcc,
                color = colors,
                size = new Point(2, 10),
                number_per_frame = 15,
                lifespan = 0.12f,
                speed = 650,
                oneTime = true
            };
            AddFX(settings, position);
        }

        public static void MineExplosion(Vector2 position) {
            Settings settings = new Settings() {
                accX = baseAcc,
                accY = baseAcc,
                color = colors,
                size = new Point(1, 10),
                number_per_frame = 15,
                lifespan = 0.12f,
                speed = 650,
                oneTime = true
            };
            AddFX(settings, position);
        }

        public static void RocketLaunch(Vector2 position) {
            Settings settings = new Settings() {
                accX = new Vector2(-1, 1),
                accY = new Vector2(-7, -2),
                color = colors,
                size = new Point(3, 13),
                number_per_frame = 12,
                lifespan = 0.12f,
                speed = 650,
                oneTime = true
            };
            AddFX(settings, position);
        }

        public static void ShootLeftExplosion(Vector2 position) {
            Settings settings = new Settings() {
                accX = new Vector2(26, 100),
                accY = new Vector2(-10, 10),
                color = colors,
                size = new Point(1, 5),
                number_per_frame = 8,
                lifespan = 0.1f,
                speed = 700,
                oneTime = true
            };
            AddFX(settings, position);
        }

         public static void ShootRightExplosion(Vector2 position) {
            Settings settings = new Settings() {
                accX = new Vector2(-100, -50),
                accY = new Vector2(-17, 9),
                color = colors,
                size = new Point(1, 5),
                number_per_frame = 8,
                lifespan = 0.1f,
                speed = 700,
                oneTime = true
            };
            AddFX(settings, position);
        }

        private static void AddFX(Settings settings, Vector2 position) {
            ParticleSystem fx = new ParticleSystem(settings, new Rectangle((int)(position.X - 10), (int)(position.Y - 10), 20, 20));
            fx.SetTexture();
            explosions.Add(fx);
        }
    }
 }