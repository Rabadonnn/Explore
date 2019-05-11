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
        public enum Screens {
            MainScreen,
            GameScreen
        }

        public static Screens currentScreen = Screens.MainScreen;

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

        public static int LeftBound {
            get {
                return -2000;
            }
        }

        public static int RightBound {
            get {
                return 2000;
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
        public static SpriteFont consolasFontBig;

        public static Player player;

        public static List<Platform> platforms;

        private static Random rand = new Random();

        private static List<BaseShip> baseShips;

        //---------UI

        private static Button playButton;
        private static Button pauseButton;
        private static Button exitButton;
        private static Button menuButton;
        private static Button resumeButton;

        private static bool isPaused = false;

        private static void Reset() {
            player = new Player();
            platforms = GenerateRandomMap();

            
            baseShips = new List<BaseShip>() {
                new BaseShip(new Vector2(0, 0)),
                new BaseShip(new Vector2(0, 0)),
                new BaseShip(new Vector2(0, 0))
            };

            DropManager.Initialize();
        }

        public static void Initialize() {
            assets = new Dictionary<string, Texture2D>();

            int buttonWidth = 100;

            playButton = new Button(new Rectangle(width / 2 - buttonWidth / 2, height / 2 - 100, buttonWidth, 50), "Play");
            pauseButton = new Button(new Rectangle(width - 100, 25, 75, 50), "Pause");
            exitButton = new Button(new Rectangle(width / 2 - buttonWidth, height / 2, 200, buttonWidth / 2), "Exit");
            menuButton = new Button(new Rectangle(width / 2 - buttonWidth / 2, height / 2 - 100, buttonWidth, 50), "MainMenu");
            resumeButton = new Button(new Rectangle(width / 2 - buttonWidth / 2, height / 2, buttonWidth, 50), "Resume");

            Reset();
        }

        private static List<Platform> GenerateRandomMap() {
            List<Platform> result = new List<Platform>();

            int minX = LeftBound;
            int maxX = RightBound;

            int platformHeight = 15;

            int minPlatformWidth = 200;
            int maxPlatformWidth = 500;

            int minHeight = 100;
            int maxHeight = 200;

            int heightBefore = 21021;

            for (int i = minX; i < maxX; i += 0) {

                int platformWidth = rand.Next(minPlatformWidth, maxPlatformWidth);

                i += platformWidth / 2;

                int heightToPlacePlatform = rand.Next(minHeight, maxHeight);

                if (Math.Abs(heightToPlacePlatform - heightBefore) < 20) {
                    heightToPlacePlatform = rand.Next(minHeight, maxHeight);
                } 
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
            consolasFontBig = contentManager.Load<SpriteFont>("ConsolasBig");

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
            assets.Add("launcher", contentManager.Load<Texture2D>("RocketLauncher"));
            assets.Add("rpg_ammo", contentManager.Load<Texture2D>("RPG_ammo"));
            assets.Add("ammo_drop", contentManager.Load<Texture2D>("AmmoDrop"));
            assets.Add("health_drop", contentManager.Load<Texture2D>("HealthDrop"));
            assets.Add("mine", contentManager.Load<Texture2D>("Mine"));
        }

        public static void SetTextures() {
            player.SetAnimations();
            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].SetTexture(assets["square"]);
            }

            for (int i = 0; i < baseShips.Count; i++) {
                baseShips[i].SetTexture(assets["dropship"]);
            }

            playButton.SetFonts();
            pauseButton.SetFonts();
            menuButton.SetFonts();
            exitButton.SetFonts();
            resumeButton.SetFonts();
        }

        public static void UpdateScreens(GameTime _gameTime) {
            gameTime = _gameTime;
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currentScreen) {
                case Screens.MainScreen:
                    UpdateMainScreen();
                    break; 

                case Screens.GameScreen:
                    UpdateGameScreen();
                    break; 
            }
        } 

        private static void UpdateMainScreen() {
            playButton.Update();
            exitButton.Update();
        }

        private static void UpdateGameScreen() {
            if (!isPaused) {
                GameObject.UpdateList();
                player.Update();
                UpdateMassObjects();
                DropManager.Update();
                DebugUpdate();

            } else if (isPaused) {
                menuButton.Update();
                resumeButton.Update();

                if (resumeButton.Clicked()) {
                    isPaused = false;
                }

                if (menuButton.Clicked()) {
                    isPaused = false;
                    currentScreen = Screens.MainScreen;
                }
            }

            pauseButton.Update();

            if (pauseButton.Clicked() && !isPaused) {
                isPaused = true;
            }
        }

        public static void DrawScreens(SpriteBatch spriteBatch) {
            DrawBackground(spriteBatch);

            switch (currentScreen) {

                case Screens.MainScreen:
                    DrawMainScreen(spriteBatch);
                    break;
                
                case Screens.GameScreen:
                    DrawGameScreen(spriteBatch);
                    break;
            }

            
            spriteBatch.Draw(Game1.camera.Debug);
        }

        private static void DrawMainScreen(SpriteBatch spriteBatch) {

            spriteBatch.Begin();

            string title = "No To Dead Yes to Everything";
            Helper.DrawString(spriteBatch, consolasFontBig, title, Color.Yellow, new Rectangle(width / 2 - 300, 100, 600, 50));

            playButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);

            if (playButton.Clicked()) {
                currentScreen = Screens.GameScreen;
            }

            spriteBatch.End();
        }

        private static void DrawGameScreen(SpriteBatch spriteBatch) {
   
            // Game

            spriteBatch.Begin(Game1.camera, samplerState: SamplerState.PointClamp);

            player.Draw(spriteBatch);

            DrawMassObjects(spriteBatch);

            DropManager.Draw(spriteBatch);

            spriteBatch.End();

            // Player HUD

            spriteBatch.Begin();

            player.DrawUI(spriteBatch);

            if (!isPaused) {
                pauseButton.Draw(spriteBatch);
            } else if (isPaused) {
                spriteBatch.Draw(assets["square"], new Rectangle(0, 0, width, height), Color.Black * 0.3f);
                menuButton.Draw(spriteBatch);
                resumeButton.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private static void DrawBackground(SpriteBatch spriteBatch) {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
           
            spriteBatch.Draw(assets["background_variant"], destinationRectangle: new Rectangle(0, 0, ScreenWidth, ScreenHeight), color: Color.White);
           
            spriteBatch.End();
        }

        public static void Print(object obj) {
            System.Diagnostics.Debug.WriteLine(obj);
        }

        private static void DebugUpdate() {
            //Print(GameObject.GetAllObjects().Count);
        }

        private static void UpdateMassObjects() {
            for (int i = 0; i < baseShips.Count; i++) {
                if (baseShips[i].isDead) {
                    baseShips.RemoveAt(i);
                } else {
                    baseShips[i].Update();
                }
            }
        }

        private static void DrawMassObjects(SpriteBatch spriteBatch) {
            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].Draw(spriteBatch);
            }

            for (int i = 0; i < baseShips.Count; i++) {
                baseShips[i].Draw(spriteBatch);
            }
        }

        public static bool ExitButtonPressed() {
            if (exitButton.Clicked()) {
                return true;
            } else {
                return false;
            }
        }
    }
}