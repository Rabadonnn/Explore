/* 
-------Comments:

publish: 
----  dotnet publish -c release -r win10-x64

*/


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Comora;
using System;
using System.Collections.Generic;

namespace Explore
{
    public static class GameManager
    {
        private static int width = 1280;
        private static int height = 720;
        public static int ScreenWidth {
            get {
                return width;
            }
        }
        public static int ScreenHeight {
            get {
                return height;
            }
        }

        public static bool isFullScreen = false;

        private static float deltaTime;
        public static float DeltaTime {
            get {
                return deltaTime;
            }
        }
        public static GameTime gameTime;

        private static Dictionary<string, Texture2D> assets;
        public static Dictionary<string, Texture2D> Assets { 
            get {
                return assets;
            }
        }
        public static SpriteFont consolasFont;

        public static Player player;

        public static List<Platform> platforms;

        private static Random rand = new Random();

        public static void Initialize() {
            assets = new Dictionary<string, Texture2D>();
            player = new Player();
            platforms = GenerateRandomMap();
        }

        private static List<Platform> GenerateRandomMap() {
            List<Platform> result = new List<Platform>();

            int minX = -GameManager.width;
            int maxX = GameManager.width;

            int platformHeight = 15;

            int minPlatformWidth = 75;
            int maxPlatformWidth = 300;

            int minHeight = 100;
            int maxHeight = 200;

            for (int i = minX; i < maxX; i += 0) {

                int platformWidth = rand.Next(minPlatformWidth, maxPlatformWidth);

                i += platformWidth / 2;

                int heightToPlacePlatform = rand.Next(minHeight, maxHeight);

                Platform platformToAdd = new Platform(new Vector2(i, heightToPlacePlatform), new Vector2(platformWidth, platformHeight));
                
                result.Add(platformToAdd);

                if (rand.Next(100) < 35) {
                    i += rand.Next(30, 100);
                } 

                i += platformWidth / 2;
            }

            return result;
        }

        public static void LoadAssets(ContentManager contentManager) {

            consolasFont = contentManager.Load<SpriteFont>("Consolas");

            assets.Add("square", contentManager.Load<Texture2D>("Square"));
            assets.Add("mamba", contentManager.Load<Texture2D>("Mamba"));
            assets.Add("crosshair", contentManager.Load<Texture2D>("Crosshair"));
            assets.Add("background", contentManager.Load<Texture2D>("Background"));
            assets.Add("kutty", contentManager.Load<Texture2D>("Kutty"));
            assets.Add("rock", contentManager.Load<Texture2D>("rock"));
            assets.Add("platform", contentManager.Load<Texture2D>("platform"));
            assets.Add("platform2", contentManager.Load<Texture2D>("platform2"));
            assets.Add("platform3", contentManager.Load<Texture2D>("platform3"));
            assets.Add("background_variant", contentManager.Load<Texture2D>("BackgroundVariant"));
            assets.Add("guard", contentManager.Load<Texture2D>("Guard"));
            assets.Add("dropship", contentManager.Load<Texture2D>("Ship"));
            assets.Add("bullet", contentManager.Load<Texture2D>("Bullet"));
            assets.Add("nuke", contentManager.Load<Texture2D>("nuke"));
            assets.Add("gun", contentManager.Load<Texture2D>("Gun"));
            assets.Add("gun2", contentManager.Load<Texture2D>("Gun2"));
        }

        public static void SetTextures() {
            player.SetAnimations();
            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].SetTexture(assets["square"]);
            }

            // for (int i = 0; i < 500; i++) {
            //     Enemy e = new Enemy(new Vector2(0, 0));
            //     e.SetAnimations();
            //     enemies.Add(e);
            // }
        }

        public static void Update(GameTime _gameTime) {
            gameTime = _gameTime;
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GameObject.UpdateList();

            player.Update();

            UpdateMassObjects();

            DebugUpdate();
        } 

        public static void Draw(SpriteBatch spriteBatch) {
            DrawBackground(spriteBatch);
            spriteBatch.Begin(Game1.camera, samplerState: SamplerState.PointClamp);

            player.Draw(spriteBatch);

            DrawMassObjects(spriteBatch);

            spriteBatch.End();

            spriteBatch.Draw(Game1.camera.Debug);
            //DrawCrosshair(spriteBatch);
        }

        private static void DrawCrosshair(SpriteBatch spriteBatch) {
            Rectangle crosshairRectangle = new Rectangle((int)Input.MouseX - 16, (int)Input.MouseY - 16, 32, 32);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(assets["crosshair"], destinationRectangle: crosshairRectangle, color: Color.White);
            spriteBatch.End();
        }

        private static void DrawBackground(SpriteBatch spriteBatch) {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
           
            spriteBatch.Draw(assets["background_variant"], destinationRectangle: new Rectangle(0, 0, ScreenWidth, ScreenHeight), color: Color.White);
           
            player.DrawUI(spriteBatch);

            spriteBatch.End();
        }

        public static void Print(object obj) {
            System.Diagnostics.Debug.WriteLine(obj);
        }

        private static void DebugUpdate() {
            //Print(GameObject.GetAllObjects().Count);
        }

        private static void UpdateMassObjects() {
            
        }

        private static void DrawMassObjects(SpriteBatch spriteBatch) {
            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].Draw(spriteBatch);
            }
        }
    }
}