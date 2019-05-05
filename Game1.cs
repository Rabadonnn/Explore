using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Comora;
using Comora.Diagnostics;

namespace Explore
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferWidth = GameManager.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameManager.ScreenHeight;
            //graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(graphics.GraphicsDevice);
            GameManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            camera.LoadContent();
            GameManager.LoadAssets(this.Content);
            GameManager.SetTextures();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            camera.Debug.IsVisible = Keyboard.GetState().IsKeyDown(Keys.F1);
            camera.Update(gameTime);
            GameManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}