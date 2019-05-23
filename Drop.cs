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
        public Drop(Vector2 _position) : base(_position) {
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

        public abstract void SetTexture();

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
        
        public override void SetTexture() {
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

        public override void SetTexture() {
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

        public override void SetTexture() {
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

        public override void SetTexture() {
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

        public override void SetTexture() {
            texture = GameManager.Assets["shield"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveShield();
        }
    }
}