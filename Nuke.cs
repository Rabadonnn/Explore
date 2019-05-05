using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Explore
{
    public class Nuke : GameObject
    {
        private int width = 20;
        private int height = 20;
        private int gravity = 300;

        public Nuke(Vector2 _position) : base("nuke") {
            position = _position;
        }

        public void Update() {

            List<Rectangle> obstacles = new List<Rectangle>();
            
            for (int i = 0; i < GameManager.platforms.Count; i++) {
                obstacles.Add(GameManager.platforms[i].rectangle);
            }

            for (int i = 0; i < GameManager.player.platforms.Count; i++) {
                obstacles.Add(GameManager.player.platforms[i].rectangle);
            } 

            for (int i = 0; i < obstacles.Count; i++) {
                if (Helper.RectRect(rectangle, obstacles[i])) {
                    isDead = true;
                }
            }

            position.Y += gravity * GameManager.DeltaTime;
            rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}