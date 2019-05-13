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
        protected System.Random rand;
        public Drop(Vector2 _position) : base("drop") {
            position = _position;
            rand = new System.Random();
        }

        public void Update() {
            if (1 == 1) {
                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), (int)width, (int)height);
            
                position.Y += gravity;

                List<Platform> platforms = GameManager.platforms;

                for (int i = 0; i < platforms.Count; i++) {
                    Rectangle obs = platforms[i].rectangle;

                    Helper.Collision collision = Helper.RectRectExtended(rectangle, obs);

                    if (collision == Helper.Collision.Bottom) {
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
            healthToGive = rand.Next(3, 6);
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

    public class AmmoDrop : Drop {
        private int ammoToGive;

        public AmmoDrop(Vector2 _position) : base(_position) {
            ammoToGive = rand.Next(15, 45);
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

    public class MinesDrop : Drop {
        private int minesToGive;
        
        public MinesDrop(Vector2 _position) : base(_position) {
            minesToGive = rand.Next(2, 5);
        }

        public void SetTexture() {
            texture = GameManager.Assets["mines_drop"];
        }

        protected override void OnPlayerPickup() {
            if (Helper.RectRect(rectangle, GameManager.player.rectangle)) {
                GameManager.player.GiveMines(minesToGive);
                isDead = true;
            }
        }
    }

    public class RocketsDrop : Drop {
        private int rocketsToGive;
        
        public RocketsDrop(Vector2 _position) : base(_position) {
            rocketsToGive = rand.Next(4, 6);
        }

        public void SetTexture() {
            texture = GameManager.Assets["rockets_drop"];
        }

        protected override void OnPlayerPickup() {
            if (Helper.RectRect(rectangle, GameManager.player.rectangle)) {
                GameManager.player.GiveRockets(rocketsToGive);
                isDead = true;
            }
        }
    }

    public static class DropManager {

        private static System.Random rand;

        private static List<HealthDrop> healthDrops;
        private static List<AmmoDrop> ammoDrops;
        private static List<MinesDrop> minesDrops;
        private static List<RocketsDrop> rocketsDrops;

        private static float initialDropCooldown = 15f;
        private static float dropCooldown;

        public static void Initialize() {
            healthDrops = new List<HealthDrop>();
            ammoDrops = new List<AmmoDrop>();
            minesDrops = new List<MinesDrop>();
            rocketsDrops = new List<RocketsDrop>();

            rand = new System.Random();

            dropCooldown = initialDropCooldown;
        }

        public static void Update() {

            if (dropCooldown <= 0) {

                Vector2 positionToDrop = NewDropPosition();

                int randInt = rand.Next(100);

                if (randInt < 25) {
                    MinesDrop md = new MinesDrop(positionToDrop);
                    md.SetTexture();
                    minesDrops.Add(md);
                } else if (randInt < 50) {
                    RocketsDrop rd = new RocketsDrop(positionToDrop);
                    rd.SetTexture();
                    rocketsDrops.Add(rd);
                } else if (randInt < 75) {
                    HealthDrop hd = new HealthDrop(positionToDrop);
                    hd.SetTexture();
                    healthDrops.Add(hd);
                } else {
                    AmmoDrop d = new AmmoDrop(positionToDrop);
                    d.SetTexture();
                    ammoDrops.Add(d);
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

            for (int  i = 0; i < rocketsDrops.Count; i++) {
                if (rocketsDrops[i].isDead) {
                    rocketsDrops.RemoveAt(i);
                } else {
                    rocketsDrops[i].Update();
                }
            }

            for (int i = 0; i < minesDrops.Count; i++) {
                if (minesDrops[i].isDead) {
                    minesDrops.RemoveAt(i);
                } else {
                    minesDrops[i].Update();
                }
            }
        }

        public static void EndOfWaveDrop() {
            Vector2 positionToDrop = NewDropPosition();

            MinesDrop md = new MinesDrop(positionToDrop);
            md.SetTexture();
            minesDrops.Add(md);

            positionToDrop = NewDropPosition();

            RocketsDrop rd = new RocketsDrop(positionToDrop);
            rd.SetTexture();
            rocketsDrops.Add(rd);

            positionToDrop = NewDropPosition();

            HealthDrop hd = new HealthDrop(positionToDrop);
            hd.SetTexture();
            healthDrops.Add(hd);

            positionToDrop = NewDropPosition();
        
            AmmoDrop d = new AmmoDrop(positionToDrop);
            d.SetTexture();
            ammoDrops.Add(d);
        }

        private static Vector2 NewDropPosition() {

            float resultX = rand.Next((int)GameManager.player.Position.X - 500, (int)GameManager.player.Position.X + 500);
            Vector2 result = new Vector2(resultX, -GameManager.ScreenHeight);
            
            List<Platform> platforms = GameManager.platforms;

            for (int i = 0; i < platforms.Count; i++) {
                
                if (result.X > platforms[i].rectangle.Left &&
                    result.X < platforms[i].rectangle.Right && 
                    result.Y < platforms[i].rectangle.Top) {
                        return result;
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

            for (int i = 0; i < rocketsDrops.Count; i++) {
                rocketsDrops[i].Draw(spriteBatch);
            }

            for (int i = 0; i < minesDrops.Count; i++) {
                minesDrops[i].Draw(spriteBatch);
            }
        }
    }
}