using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Explore
{
    public abstract class Drop : GameObject
    {
        protected int width = 32;
        protected int height = 32;
        protected int gravity = 10;
        protected bool isGrounded = false;
        public Drop(Vector2 _position) : base("drop") {
            position = _position;
        }

        public void Update() {
            if (1 == 1) {
                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), (int)width, (int)height);
            
                position.Y += gravity;

                List<Platform> platforms = GameManager.platforms;

                for (int i = 0; i < platforms.Count; i++) {
                    Rectangle obs = platforms[i].rectangle;

                    string collision = Helper.RectangleCollision(rectangle, obs);

                    if (collision == "bottom") {
                        position.Y = obs.Top - width / 2;
                        isGrounded = true;
                    }
                }
            }

            OnPlayerPickup();
        }

        protected abstract void OnPlayerPickup();

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }

    public class HealthDrop : Drop {

        private int healthToGive;

        public HealthDrop(Vector2 _position) : base(_position) {
            healthToGive = 3;
        }
        
        public void SetTexture() {
            texture = GameManager.Assets["health_drop"];
        }

        protected override void OnPlayerPickup() {
            if (Helper.RectRect(rectangle, GameManager.player.rectangle)) {
                GameManager.player.GiveHealth(healthToGive);
                isDead = true;
            }
        }
    }

    public class AmmoDrop : Drop
    {

        private int ammoToGive;

        public AmmoDrop(Vector2 _position) : base(_position) {
            ammoToGive = 30;
        }

        public void SetTexture() {
            texture = GameManager.Assets["ammo_drop"];
        }

        protected override void OnPlayerPickup() {
            if (Helper.RectRect(rectangle, GameManager.player.rectangle)) {
                GameManager.player.GiveHandGunAmmo(ammoToGive);
                isDead = true;
            }
        }
    }

    public static class DropManager {

        private static System.Random rand;

        private static List<HealthDrop> healthDrops;
        private static List<AmmoDrop> ammoDrops;

        private static float initialDropCooldown = 1f;
        private static float dropCooldown;

        public static void Initialize() {
            healthDrops = new List<HealthDrop>();
            ammoDrops = new List<AmmoDrop>();

            rand = new System.Random();

            dropCooldown = initialDropCooldown;
        }

        public static void Update() {

            if (dropCooldown <= 0) {
                Vector2 positionToDrop = NewDropPosition();

                if (rand.Next(100) < 50) {
                    HealthDrop hd = new HealthDrop(positionToDrop);
                    hd.SetTexture();
                    healthDrops.Add(hd);
                } else {
                    AmmoDrop ad = new AmmoDrop(positionToDrop);
                    ad.SetTexture();
                    ammoDrops.Add(ad);
                }

                dropCooldown = initialDropCooldown;

            } else {
                dropCooldown -= GameManager.DeltaTime;
            }

            for (int i = 0; i < healthDrops.Count; i++) {
                if (healthDrops[i].isDead) {
                    healthDrops.RemoveAt(i);
                } else {
                    healthDrops[i].Update();
                }
            }

            for (int i = 0; i < ammoDrops.Count; i++) {
                if (ammoDrops[i].isDead) {
                    ammoDrops.RemoveAt(i);
                } else {
                    ammoDrops[i].Update();
                }
            }
        }

        private static Vector2 NewDropPosition() {

            bool foundPosition = false;

            Vector2 result = new Vector2(rand.Next(-GameManager.ScreenWidth, GameManager.ScreenWidth), rand.Next(-GameManager.ScreenHeight, 0));
            
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                if (foundPosition) {
                    break;
                } else {
                    if (result.X > platforms[i].rectangle.Left &&
                        result.X < platforms[i].rectangle.Right && 
                        result.Y < platforms[i].rectangle.Top) {
                            return result;
                        }
                }
            }

            return new Vector2();
        }

        public static void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < healthDrops.Count; i++) {
                healthDrops[i].Draw(spriteBatch);
            }

            for (int i = 0; i < ammoDrops.Count; i++) {
                ammoDrops[i].Draw(spriteBatch);
            }
        }
    }
}