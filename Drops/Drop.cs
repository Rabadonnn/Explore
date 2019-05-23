using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Explore.Drops
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
}