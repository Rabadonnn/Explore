using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Explore
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

        private static List<BaseShip> baseShips;

        private static List<BaseEnemy> baseEnemies;

        private static List<BombShip> bombShips;

        private static Random rand;

        private static float initalBombShipCooldown = 10f;
        private static float bombShipCooldown = 5f;

        public static void Init() {
            waveNumber = 0;
            firstWave = true;
            baseShips = new List<BaseShip>();
            baseEnemies = new List<BaseEnemy>();
            bombShips = new List<BombShip>();
            rand = new Random();
        }

        public static void Update() {

            if (firstWave) {
                for (int i = 0; i < 3; i++) {
                    NewShip();
                }
                firstWave = false;
            }

            for (int i = 0; i < baseShips.Count; i++) {
                if (baseShips[i].isDead) {
                    baseShips.RemoveAt(i);
                } else {
                    baseShips[i].Update();
                }
            }

            for (int i = 0; i < baseEnemies.Count; i++) {
                if (baseEnemies[i].isDead) {
                    baseEnemies.RemoveAt(i);
                } else {
                    baseEnemies[i].Update();
                }
            }

            for (int i = 0; i < bombShips.Count; i++) {
                if (bombShips[i].isDead) {
                    bombShips.RemoveAt(i);
                } else {
                    bombShips[i].Update();
                }
            }

            UpdateWaves();
        }

        private static void UpdateWaves() {
            if (baseShips.Count == 0) {
                waveNumber++;
                for (int i = 0; i < 3 + rand.Next(waveNumber); i++) {
                    NewShip();
                }
                DropManager.EndOfWaveDrop();
            }

            if (bombShipCooldown <= 0) {
                NewBombShip();
                bombShipCooldown = initalBombShipCooldown;
            } else {
                bombShipCooldown -= GameManager.DeltaTime;
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {

            for (int i = 0; i < baseShips.Count; i++) {
                baseShips[i].Draw(spriteBatch);
            }

            for (int i = 0; i < baseEnemies.Count; i++) {
                baseEnemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < bombShips.Count; i++) {
                bombShips[i].Draw(spriteBatch);
            }
        }

        public static void AddBaseEnemy(BaseEnemy e) {
            baseEnemies.Add(e);
        }
        
        private static void NewShip() {
            BaseShip s = new BaseShip(new Vector2(rand.Next(-300, 300), 0));
            s.SetTexture();
            baseShips.Add(s);
        }

        private static void NewBombShip() {
            BombShip b = new BombShip(new Vector2(rand.Next(-300, 300), 0));
            b.SetTexture();
            bombShips.Add(b);
        }
    }
}