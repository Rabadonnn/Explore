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

        private static Random rand;

        public static void Init() {
            waveNumber = 0;
            firstWave = true;
            baseShips = new List<BaseShip>();
            baseEnemies = new List<BaseEnemy>();
            rand = new Random();
        }

        public static void Update() {

            if (firstWave) {
                for (int i = 0; i < 3; i++) {
                    NewShip(new Vector2());
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

            UpdateWaves();
        }

        private static void UpdateWaves() {
            if (baseShips.Count == 0) {
                waveNumber++;
                for (int i = 0; i < 3 + rand.Next(waveNumber); i++) {
                    NewShip(new Vector2(rand.Next(-300, 300), 0));
                }
                DropManager.EndOfWaveDrop();
            }
        }

        public static void Draw(SpriteBatch spriteBatch) {

            for (int i = 0; i < baseShips.Count; i++) {
                baseShips[i].Draw(spriteBatch);
            }

            for (int i = 0; i < baseEnemies.Count; i++) {
                baseEnemies[i].Draw(spriteBatch);
            }
        }

        public static void AddBaseEnemy(BaseEnemy e) {
            baseEnemies.Add(e);
        }
        
        private static void NewShip(Vector2 posiition) {
            BaseShip s = new BaseShip(new Vector2(rand.Next(-300, 300), 0));
            s.SetTexture();
            baseShips.Add(s);
        }
    }
}