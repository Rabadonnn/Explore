using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Explore
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Rectangle rectangle;
        public Rectangle Rectangle {
            get {
                return rectangle;
            }
        }
        protected Texture2D texture;
        public bool isDead = false;

        public GameObject() {
            
        }

        public GameObject(Vector2 pos) {
            position = pos;
        }
        
        public virtual void SetTexture(Texture2D _texture) {
            texture = _texture;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}