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

        public override void Update() {
            if (1 == 1) {
                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), (int)width, (int)height);
            
                position.Y += gravity;

                List<Platform> platforms = GameManager.platforms;

                for (int i = 0; i < platforms.Count; i++) {
                    Rectangle obs = platforms[i].Rectangle;

                    Helper.Collision collision = Helper.RectRectExtended(rectangle, obs);

                    if (collision == Helper.Collision.Bottom) {
                        position.Y = obs.Top - width / 2;
                        isGrounded = true;
                    }
                }
            }
            if (Helper.RectRect(rectangle, GameManager.player.Rectangle)) {
                OnPlayerPickup();
                isDead = true;
            }
        }

        protected abstract void OnPlayerPickup();

        public override void Draw(SpriteBatch spriteBatch) {
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
            GameManager.player.GiveHealth(healthToGive);
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
            GameManager.player.GiveHandGunAmmo(ammoToGive);
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
            GameManager.player.GiveMines(minesToGive);
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
            GameManager.player.GiveRockets(rocketsToGive);
        }
    }

    public class ShieldDrop : Drop {

        public ShieldDrop(Vector2 _position) : base(_position) {
            width = 32;
            height = 32;
        }

        public void SetTexture() {
            texture = GameManager.Assets["shield"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveShield();
        }
    }

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

                if (randInt < 25) {
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
                Vector2 positionToDrop = NewDropPosition();

               NewDrop<ShieldDrop>(positionToDrop);

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
                (d as MinesDrop).SetTexture();
            } else if (typeof(T) == typeof(RocketsDrop)) {
                d = new RocketsDrop(position);
                (d as RocketsDrop).SetTexture();
            } else if (typeof(T) == typeof(AmmoDrop)) {
                d = new AmmoDrop(position);
                (d as AmmoDrop).SetTexture();
            } else if (typeof(T) == typeof(HealthDrop)) {
                d = new HealthDrop(position);
                (d as HealthDrop).SetTexture();
            } else if (typeof(T) == typeof(ShieldDrop)) {
                d = new ShieldDrop(position);
                (d as ShieldDrop).SetTexture();
            } else {
                d = null;
            }
            
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