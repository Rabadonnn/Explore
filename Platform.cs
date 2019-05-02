using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Explore
{
    public class Platform : GameObject
    {
        private float lifeTime = 3;

        public Rectangle Rectangle {
            get {
                return rectangle;
            }
        }
        public Platform(Vector2 _position, Vector2 _size) : base(_position, "platform") {
            rectangle = new Rectangle((int)(_position.X - _size.X / 2), (int)(_position.Y - _size.Y / 2), (int)_size.X, (int)_size.Y);
            position = _position;
        }

        public void Update() {
            lifeTime -= GameManager.DeltaTime;

            if (lifeTime <= 0) {
                isDead = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture: texture, destinationRectangle: rectangle, color: Color.DarkGray);
        }
    }
}