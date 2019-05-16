using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore
{
    public class Shield : GameObject
    {
        private int strength;
        private int initialStrength;
        public int Strength {
            get {
                return strength;
            }
            set {
                initialStrength = value;
            }
        }

        private float lifetime;
        private float initialLifeTime;
        public float Lifetime {
            set {
                initialLifeTime = value;
            }
        }

        private int width = 64;
        private int height = 64;

        private bool on = false;
        public bool Active {
            get {
                return on;
            }
        }


        public Shield() : base("shield") {

        }

        public void SetTexture() {
            texture = GameManager.Assets["shield"];
        }

        public override void Update() {

        }

        public void Update(Vector2 _position) {
            if (on) {
                rectangle = new Rectangle((int)(position.X - width / 2), (int)(position.Y - height / 2), width, height);
                position = _position;

                if (lifetime >= 0) {
                    lifetime -= GameManager.DeltaTime;
                }

                if (strength <= 0 || lifetime <= 0) {
                    on = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (on) {
                spriteBatch.Draw(texture, rectangle, Color.White);  
            }
        } 

        public void Start() {
            on = true;
            strength = initialStrength;
            lifetime = initialLifeTime;
        }

        public void AddLifetime(float amount) {
            lifetime += amount;
        }

        public void Damage(int amount) {
            strength -= amount;
        }
    }
}