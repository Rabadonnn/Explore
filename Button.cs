using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Explore
{
    public class Button
    {
        private Rectangle rectangle;

        private string text;
        private string hoverText;
        private string currentText;

        private Color currentColor;
        private Color baseColor;
        private Color hoverColor;

        private SpriteFont currentFont;
        private SpriteFont baseFont;
        private SpriteFont hoverFont;

        private bool alreadyClicked;

        public bool active = true;
        
        public Button(Rectangle _rectangle, string _text) {
            rectangle = _rectangle;

            text = _text;
            hoverText = ">" + text + "<";

            currentText = text;

            baseColor = Color.White;
            hoverColor = Color.Yellow;
        }

        public void SetFonts() {
            currentColor = baseColor;

            baseFont = GameManager.consolasFont;
            hoverFont = GameManager.consolasFontBig;

            currentFont = baseFont;
        }

        public void Update() {
            if (IsHovered && currentColor != hoverColor) {
                currentColor = hoverColor;
                currentFont = hoverFont;
                currentText = hoverText;
            } else if (!IsHovered && currentColor != baseColor) {
                currentColor = baseColor;
                currentFont = baseFont;
                currentText = text;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Helper.DrawString(spriteBatch, hoverFont, text, currentColor, rectangle);
        }

        private bool IsHovered {
            get {
                return 
                    Input.MouseX > rectangle.Left && 
                    Input.MouseX < rectangle.Right &&
                    Input.MouseY > rectangle.Top && 
                    Input.MouseY < rectangle.Bottom;
            }
        }

        public bool Clicked() {
            if (IsHovered && Input.LeftClick && !alreadyClicked && active) {
                alreadyClicked = true;
                return true;
            } else {
                if (Input.LeftClickReleased) {
                    alreadyClicked = false;
                }
            }
            return false;
        }
    }
}