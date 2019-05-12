/* 
-------Comments:

publish: 
----  dotnet publish -c release -r win10-x64

Just Win motherFucker

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

        // Game Continuity

        private static int waveNumber = 0;
        public static int WaveNumver {
            get {
                return waveNumber;
            }
        }

        //---------UI

        private static Dictionary<string, Rectangle> buttonRectangles;

        private static Button playButton;
        private static Button pauseButton;
        private static Button exitButton;
        private static Button menuButton;
        private static Button resumeButton;

        private static Dictionary<string, Button> resolutionButtons;
        private static Dictionary<string, Point> resolutions;

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

        private static void MakeRectanglesForButtons() {
            int buttonWidth = 100;

            buttonRectangles = new Dictionary<string, Rectangle>() {
                {
                    "play", new Rectangle(width / 2 - buttonWidth / 2, height / 2 - 100, buttonWidth, 50)
                },
                {
                    "pause", new Rectangle(width - 100, 25, 75, 50)
                },
                {
                    "exit", new Rectangle(width / 2 - buttonWidth, height / 2, 200, buttonWidth / 2)
                },
                {
                    "menu", new Rectangle(width / 2 - buttonWidth / 2, height / 2 - 100, buttonWidth, 50)
                },
                {
                    "resume", new Rectangle(width / 2 - buttonWidth / 2, height / 2 , buttonWidth, 50)
                }
            };
        }

        public static void Initialize() {
            assets = new Dictionary<string, Texture2D>();

            MakeRectanglesForButtons();

            playButton = new Button(buttonRectangles["play"], "Play");
            pauseButton = new Button(buttonRectangles["pause"], "Pause");
            exitButton = new Button(buttonRectangles["exit"], "Exit");
            menuButton = new Button(buttonRectangles["menu"], "MainMenu");
            resumeButton = new Button(buttonRectangles["resume"], "Resume");

            resolutionButtons = new Dictionary<string, Button>() {
                {
                    "fullscreen", new Button(new Rectangle(100, height / 2 - 100, 100, 50), "FullScreen")
                },
                {
                    "1920x1080", new Button(new Rectangle(100, height / 2 - 50, 100, 50), "1920x1080")
                }, 
                {
                    "1600x900", new Button(new Rectangle(100, height / 2, 100, 50), "1600x900")
                }, 
                {
                    "1280x720", new Button(new Rectangle(100, height / 2 + 50, 100, 50), "1280x720")
                },
                {
                    "1024x768", new Button(new Rectangle(100, height / 2 + 100, 100, 50), "1024x768")
                },
                {
                    "800x600", new Button(new Rectangle(100, height / 2 + 150, 100, 50), "800x600")
                }
            };

            resolutions = new Dictionary<string, Point>() {
                {
                    "1920x1080", new Point(1920, 1080)
                }, 
                {
                    "1600x900", new Point(1600, 900)
                },
                {
                    "1280x720", new Point(1280, 720)
                },
                {
                    "1024x768", new Point(1024, 768)
                },
                {
                    "800x600", new Point(800, 600)
                }
            };

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
            assets.Add("mines_drop", contentManager.Load<Texture2D>("MinesDrop"));
            assets.Add("rockets_drop", contentManager.Load<Texture2D>("RocketsDrop"));
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

            foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                entry.Value.SetFonts();
            }
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

            foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                entry.Value.Update();
            }

            if (playButton.Clicked()) {
                exitButton.active = false;

                foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                    entry.Value.active = false;
                }
                currentScreen = Screens.GameScreen;
            }
        }

        private static void UpdateGameScreen() {
            if (!isPaused) {
                GameObject.UpdateList();
                player.Update();
                UpdateMassObjects();
                DropManager.Update();
                DebugUpdate();

                ManageWaves();

            } else if (isPaused) {
                menuButton.Update();
                resumeButton.Update();

                if (menuButton.Clicked()) {
                    isPaused = false;
                    currentScreen = Screens.MainScreen;
                    exitButton.active = true;

                    foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                        entry.Value.active = true;
                    }
                }

                if (resumeButton.Clicked()) {
                    isPaused = false;
                }
            }

            pauseButton.Update();

            if (pauseButton.Clicked() && !isPaused) {
                isPaused = true;
            }

            if (Input.Esc && !isPaused) {
                isPaused = true;
            }
        }

        private static void ManageWaves() {
            if (baseShips.Count == 0) {
                for (int i = 0; i < waveNumber + 3; i++) {
                    BaseShip b = new BaseShip(new Vector2(rand.Next(LeftBound, RightBound), -300));
                    b.SetTexture(assets["dropship"]);
                    baseShips.Add(b);
                }
                waveNumber++;
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

            foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                entry.Value.Draw(spriteBatch);
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

            Helper.DrawString(spriteBatch, consolasFontBig, "Wave: " + (waveNumber + 1).ToString(), Color.White, new Rectangle(width / 2 - 50, 10, 100, 40));

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

        public static void CheckForResolutionChanges(GraphicsDeviceManager graphics) {
            if (resolutionButtons["fullscreen"].Clicked()) {
                if (graphics.IsFullScreen) {
                    graphics.IsFullScreen = false;
                } else if (!graphics.IsFullScreen) {
                    graphics.IsFullScreen = true;
                }
                graphics.ApplyChanges();

            } 

            foreach (KeyValuePair <string, Button> entry in resolutionButtons) {
                if (entry.Value.Clicked()) {
                    ChangeResolution(graphics, resolutions[entry.Key]);
                }
            }
        }

        private static void ChangeResolution(GraphicsDeviceManager graphics, Point size) {
            if (graphics.PreferredBackBufferWidth != size.X && graphics.PreferredBackBufferHeight != size.Y) {
                graphics.PreferredBackBufferWidth = size.X;
                graphics.PreferredBackBufferHeight = size.Y;
                width = size.X;
                height = size.Y;

                MakeRectanglesForButtons();

                playButton.UpdateRectangle(buttonRectangles["play"]);
                pauseButton.UpdateRectangle(buttonRectangles["pause"]);
                menuButton.UpdateRectangle(buttonRectangles["menu"]);
                exitButton.UpdateRectangle(buttonRectangles["exit"]);
                resumeButton.UpdateRectangle(buttonRectangles["resume"]);
                
                graphics.ApplyChanges();
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