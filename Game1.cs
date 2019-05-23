using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Comora;

namespace Explore
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Camera camera;

        public Game1()
        {
            Config.Load();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //graphics.IsFullScreen = true;
            IsFixedTimeStep = Config.Game["fixedTimeStep"].BoolValue;
            graphics.SynchronizeWithVerticalRetrace = Config.Game["vsync"].BoolValue;
            graphics.PreferredBackBufferWidth = GameManager.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameManager.ScreenHeight;
            //graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            camera = new Camera(graphics.GraphicsDevice);
            GameManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera.LoadContent();
            GameManager.LoadAssets(this.Content);
            GameManager.SetTextures();
        }

        protected override void Update(GameTime gameTime)
        {
            camera.Debug.IsVisible = Keyboard.GetState().IsKeyDown(Keys.F1);
            camera.Update(gameTime);
            GameManager.UpdateScreens(gameTime);

            if (GameManager.ExitButtonPressed()) {
                Exit();
            }

            GameManager.CheckForResolutionChanges(graphics);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameManager.DrawScreens(spriteBatch);

            base.Draw(gameTime);
        }

        
    }
}