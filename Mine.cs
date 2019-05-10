using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Spritesheet;

namespace Explore
{
    public class Mine : GameObject
    {
        private int width = 32;
        private int height = 20;

        private Vector2 velocity;

        private bool placed;
        public bool IsPlaced {
            get {
                return placed;
            }
        }

        private int gravity = 10;

        private int speed = 350;

        private Spritesheet.Spritesheet spritesheet;

        private Animation animation;

        public Mine(Vector2 _position, int direction) : base("mine") {
            position = _position;

            velocity = new Vector2(speed * direction, 0);

            placed = false;
        } 

        public void SetAnimations() {
            spritesheet = new Spritesheet.Spritesheet(GameManager.Assets["mine"]).WithGrid((16, 10));

            animation = spritesheet.CreateAnimation((0, 0), (1, 0), (2, 0));
            animation.Start(Repeat.Mode.LoopWithReverse);
        }

        public void Update() {

            if (!placed) {
                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);

                position.Y += gravity;

                position += velocity * GameManager.DeltaTime;

                List<Platform> platforms = GameManager.platforms;

                for (int i = 0; i < platforms.Count; i++) {
                    Helper.Collision collision = Helper.RectangleCollision(rectangle, platforms[i].rectangle);

                    if (collision == Helper.Collision.Bottom) {
                        velocity.Y = 0;
                        placed = true;
                        position.Y = platforms[i].rectangle.Top - height / 2;
                    }
                }
            }

            animation.Update(GameManager.gameTime);
        }

        public void Explode() {
            isDead = true;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(animation, position, null, 0, new Vector2(2, 2), 0);
        }
    }
}