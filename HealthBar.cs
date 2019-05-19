using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore
{
    public class HealthBar
    {
        private Texture2D texture;
        private Rectangle rectangle1;
        private Rectangle rectangle2;
        private int width = 40;
        private int height = 7;
        private Color color1 = Color.Green;
        private Color color2 = Color.Red;

        public void SetTexture() => texture = GameManager.Assets["square"];

        public void Update(Vector2 _position, int maxHp, int currentHp) {
            int rect1Width = (int)Helper.MapValue(currentHp, maxHp, 0, width, 0);
            rectangle1 = new Rectangle((int)(_position.X - width / 2), (int)(_position.Y - height / 2), rect1Width, height);
            rectangle2 = new Rectangle(rectangle1.Right, rectangle1.Top, width - rectangle1.Width, height);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle1, color1);
            spriteBatch.Draw(texture, rectangle2, color2);
        }
    }
}