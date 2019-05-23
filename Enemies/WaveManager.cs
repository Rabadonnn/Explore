using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Explore.Drops;

namespace Explore.Enemies
{
    public static class WaveManager
    {
        private static int waveNumber;
        public static int WaveNumber {
            get {
                return waveNumber;
            }
        }

        private static bool firstWave;

        private static List<GameObject> enemies;

        private static Random rand;

        private static float initalBombShipCooldown;
        private static float bombShipCooldown;

        public static void Init() {
            waveNumber = 0;
            firstWave = true;
            enemies = new List<GameObject>();
            rand = new Random();
            initalBombShipCooldown = rand.Next(3, 10);
        }

        public static void Update() {

            if (firstWave) {
                for (int i = 0; i < 3; i++) {
                    NewShip();
                }
                firstWave = false;
            }

            foreach (var e in enemies.ToArray()) {
                if (e.isDead) {
                    if (e is BaseShip) {
                        SoundManager.ShipExplosion();
                    }
                    enemies.Remove(e);
                } else {
                    e.Update();
                }
            }

            UpdateWaves();
        }

        private static void UpdateWaves() {
            if (enemies.Count == 0) {
                waveNumber++;
                for (int i = 0; i < rand.Next(3, 3 + waveNumber); i++) {
                    NewShip();
                }
                DropManager.EndOfWaveDrop();
            }

            if (bombShipCooldown <= 0) {
                int bombShipCount = 0;
                for (int i = 0; i < enemies.Count; i++) {
                    if (enemies[i] is Bombship) {
                        bombShipCount++;
                    }
                }
                if (bombShipCount < 4) {
                    NewBombShip();
                    bombShipCooldown = rand.Next(3, 10);
                }
            } else {
                bombShipCooldown -= GameManager.DeltaTime;
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].Draw(spriteBatch);
            }
        }

        public static void AddBaseEnemy(BaseEnemy e) {
            enemies.Add(e);
        }
        
        private static void NewShip() {
            BaseShip s = new BaseShip(new Vector2(rand.Next(-300, 300), 0));
            s.SetTexture();
            enemies.Add(s);
        }

        private static void NewBombShip() {
            BaseShip b = new Bombship(new Vector2(rand.Next((int)GameManager.player.Position.X - 400, (int)GameManager.player.Position.X + 400), rand.Next(-300, -100)));
            (b as Bombship).SetTexture();
            enemies.Add(b);
        }
    }
}