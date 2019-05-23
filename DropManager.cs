using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Explore
{
    public static class DropManager {

        private static System.Random rand;

        private static List<Drop> drops;

        private static float initialDropCooldown = 15f;
        private static float dropCooldown;

        private static float initialShieldDropCooldown = 20f;
        private static float shieldDropCooldown = 0;

        public static void Initialize() {
            drops = new List<Drop>();

            rand = new System.Random();

            dropCooldown = initialDropCooldown;
            shieldDropCooldown = initialShieldDropCooldown;
        }

        public static void Update() {

            if (dropCooldown <= 0) {

                Vector2 positionToDrop = NewDropPosition();

                int randInt = rand.Next(100);

                if (GameManager.player.RocketsCount < 3) {
                    NewDrop<RocketsDrop>(positionToDrop);
                } else if (randInt < 25) {
                    NewDrop<MinesDrop>(positionToDrop);
                } else if (randInt < 50) {
                    NewDrop<RocketsDrop>(positionToDrop);
                } else if (randInt < 75) {
                    NewDrop<HealthDrop>(positionToDrop);
                } else {
                    NewDrop<AmmoDrop>(positionToDrop);
                }

                dropCooldown = initialDropCooldown;

            } else {
                dropCooldown -= GameManager.DeltaTime;
            }

            if (shieldDropCooldown <= 0) {
                bool canDrop = true;

                foreach (var d in drops) {
                    if (d is ShieldDrop) {
                        canDrop = false;
                    }
                }

                if (canDrop) {
                    Vector2 positionToDrop = NewDropPosition();
                    NewDrop<ShieldDrop>(positionToDrop);
                }

                shieldDropCooldown = initialShieldDropCooldown;
            } else {
                shieldDropCooldown -= GameManager.DeltaTime;
            }

            for (int i = 0; i < drops.Count; i++) {
                if (drops[i].isDead) {
                    drops.RemoveAt(i);
                } else {
                    drops[i].Update();
                }
            }
        }

        public static void EndOfWaveDrop() {
            Vector2 positionToDrop = NewDropPosition();

            NewDrop<MinesDrop>(positionToDrop);

            positionToDrop = NewDropPosition();

            NewDrop<RocketsDrop>(positionToDrop);

            positionToDrop = NewDropPosition();

            NewDrop<HealthDrop>(positionToDrop);

            positionToDrop = NewDropPosition();
        
            NewDrop<AmmoDrop>(positionToDrop);

            positionToDrop = NewDropPosition();

            NewDrop<ShieldDrop>(positionToDrop);
        }

        private static Drop NewDrop<T>(Vector2 position) {
            Drop d;

            if (typeof(T) == typeof(MinesDrop)) {
                d = new MinesDrop(position);
            } else if (typeof(T) == typeof(RocketsDrop)) {
                d = new RocketsDrop(position);
            } else if (typeof(T) == typeof(AmmoDrop)) {
                d = new AmmoDrop(position);
            } else if (typeof(T) == typeof(HealthDrop)) {
                d = new HealthDrop(position);
            } else if (typeof(T) == typeof(ShieldDrop)) {
                d = new ShieldDrop(position);
            } else {
                d = null;
            }

            d.SetTexture();
            
            drops.Add(d);

            return d;
        }

        private static Vector2 NewDropPosition() {

            float resultX = rand.Next((int)GameManager.player.Position.X - 500, (int)GameManager.player.Position.X + 500);
            Vector2 result = new Vector2(resultX, -GameManager.ScreenHeight);
            
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                
                if (result.X > platforms[i].Rectangle.Left &&
                    result.X < platforms[i].Rectangle.Right && 
                    result.Y < platforms[i].Rectangle.Top) {
                        return result;
                    }
            }

            return new Vector2();
        }

        public static void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < drops.Count; i++) {
                drops[i].Draw(spriteBatch);
            }
        }
    }
}