using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Explore
{
    public class Platform : GameObject
    {
        private float lifeTime = 3;

        public Platform(Vector2 _position, Vector2 _size) : base(_position) {
            rectangle = Helper.MakeRectangle(position, (int)_size.X, (int)_size.Y);
            position = _position;
        }

        public override void Update() {
            lifeTime -= GameManager.DeltaTime;

            if (lifeTime <= 0) {
                isDead = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture: texture, destinationRectangle: rectangle, color: Color.DarkGray);
        }
    }
}