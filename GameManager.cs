/* 
-------Comments:

publish: 
----  dotnet publish -c release -r win10-x64

Just Win MotherFucker

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
            GameScreen,
            OptionsScreen
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


        //---------UI

        private static Dictionary<string, Rectangle> buttonRectangles;

        private static Button playButton;
        private static Button pauseButton;
        private static Button exitButton;
        private static Button menuButton;
        private static Button optionsButton;
        private static Button resumeButton;
        private static Button backButton;
        private static Button replayButton;

        private static Dictionary<string, Button> resolutionButtons;
        private static Dictionary<string, Point> resolutions;

        private static bool isPaused = false;

        private static void Reset() {
            platforms = GenerateRandomMap();

            DropManager.Initialize();

            SetTextures();

            player.Reset();
            WaveManager.Init();
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
                    "exit", new Rectangle(width / 2 - buttonWidth, height / 2 + 100, 200, buttonWidth / 2)
                },
                {
                    "menu", new Rectangle(width / 2 - buttonWidth / 2, height / 2 - 100, buttonWidth, 50)
                },
                {
                    "resume", new Rectangle(width / 2 - buttonWidth / 2, height / 2 , buttonWidth, 50)
                },
                {
                    "back", new Rectangle(width - 100, 25, 75, 50)
                },
                {
                    "options", new Rectangle(width / 2 - buttonWidth, height / 2, 200, buttonWidth / 2)
                }
            };
        }

        public static void Initialize() {
            assets = new Dictionary<string, Texture2D>();

            player = new Player();

            WaveManager.Init();

            MakeRectanglesForButtons();

            playButton = new Button(buttonRectangles["play"], "Play");
            pauseButton = new Button(buttonRectangles["pause"], "Pause");
            exitButton = new Button(buttonRectangles["exit"], "Exit");
            menuButton = new Button(buttonRectangles["menu"], "MainMenu");
            resumeButton = new Button(buttonRectangles["resume"], "Resume");
            backButton = new Button(buttonRectangles["back"], "Back");
            optionsButton = new Button(buttonRectangles["options"], "Options");
            replayButton = new Button(buttonRectangles["play"], "Replay");

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

            platforms = GenerateRandomMap();

            DropManager.Initialize();
        }

        private static List<Platform> GenerateRandomMap() {
            List<Platform> result = new List<Platform>();

            int minX = LeftBound;
            int maxX = RightBound;

            int platformHeight = 15;

            int minPlatformWidth = 350;
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

                i += rand.Next(50, 100);
                

                i += platformWidth / 2;
            }

            return result;
        }

        public static void LoadAssets(ContentManager contentManager) {

            consolasFont = contentManager.Load<SpriteFont>("Consolas");
            consolasFontBig = contentManager.Load<SpriteFont>("ConsolasBig");

            assets.Add("square", contentManager.Load<Texture2D>("Square"));
            assets.Add("mamba", contentManager.Load<Texture2D>("Mamba"));
            assets.Add("platform3", contentManager.Load<Texture2D>("platform3"));
            assets.Add("background_variant", contentManager.Load<Texture2D>("BackgroundVariant"));
            assets.Add("guard", contentManager.Load<Texture2D>("Guard"));
            assets.Add("dropship", contentManager.Load<Texture2D>("Ship"));
            assets.Add("bullet", contentManager.Load<Texture2D>("Bullet"));
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

            playButton.SetFonts();
            pauseButton.SetFonts();
            menuButton.SetFonts();
            exitButton.SetFonts();
            resumeButton.SetFonts();
            optionsButton.SetFonts();
            backButton.SetFonts();
            replayButton.SetFonts();

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
                
                case Screens.OptionsScreen:
                    UpdateOptionsScreen();
                    break;
            }
        } 

        private static void UpdateMainScreen() {
            playButton.Update();
            optionsButton.Update();
            exitButton.Update();

            if (playButton.Clicked()) {
                exitButton.active = false;

                currentScreen = Screens.GameScreen;
            }

            if (optionsButton.Clicked()) {
                exitButton.active = false;

                foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                    entry.Value.active = true;
                }

                backButton.active = true;
                currentScreen = Screens.OptionsScreen;
            }
        }

        private static void UpdateGameScreen() {
            if (!isPaused) {
                GameObject.UpdateList();
        
                if (player.isDead) {
                    replayButton.active = true;
                    replayButton.Update();

                    if (replayButton.Clicked()) {
                        Reset();
                        replayButton.active = false;
                    }
                } else {
                    player.Update();

                    DropManager.Update();

                    WaveManager.Update();

                    DebugUpdate();
                }
            } else if (isPaused) {
                menuButton.Update();
                resumeButton.Update();

                if (menuButton.Clicked()) {
                    isPaused = false;
                    currentScreen = Screens.MainScreen;
                    exitButton.active = true;
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

        private static void UpdateOptionsScreen() {
            backButton.Update();

            foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                entry.Value.Update();
            }

            if (backButton.Clicked()) {
                backButton.active = false;
                foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                    entry.Value.active = false;
                }
                exitButton.active = true;
                currentScreen = Screens.MainScreen;
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

                case Screens.OptionsScreen:
                    DrawOptionsScreen(spriteBatch);
                    break;
            }

            spriteBatch.Draw(Game1.camera.Debug);
        }

        private static void DrawMainScreen(SpriteBatch spriteBatch) {

            spriteBatch.Begin();

            string title = "No To Death Yes to Everything";
            Helper.DrawString(spriteBatch, consolasFontBig, title, Color.Yellow, new Rectangle(width / 2 - 300, 100, 600, 100));

            playButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        private static void DrawGameScreen(SpriteBatch spriteBatch) {
   
            // Game

            spriteBatch.Begin(Game1.camera, samplerState: SamplerState.PointClamp);

            player.Draw(spriteBatch);

            DrawPlatforms(spriteBatch);

            DropManager.Draw(spriteBatch);

            WaveManager.Draw(spriteBatch);

            spriteBatch.End();

            // Player HUD

            spriteBatch.Begin();

            player.DrawUI(spriteBatch);

            if (!isPaused) {
                pauseButton.Draw(spriteBatch);

                if (player.isDead) {
                    replayButton.Draw(spriteBatch);
                }
            } else if (isPaused) {
                spriteBatch.Draw(assets["square"], new Rectangle(0, 0, width, height), Color.Black * 0.3f);
                menuButton.Draw(spriteBatch);
                resumeButton.Draw(spriteBatch);
            }

            Helper.DrawString(spriteBatch, consolasFontBig, "Wave: " + (WaveManager.WaveNumber + 1).ToString(), Color.White, new Rectangle(width / 2 - 50, 10, 100, 40));

            spriteBatch.End();
        }

        private static void DrawOptionsScreen(SpriteBatch spriteBatch) {

            spriteBatch.Begin();

            foreach (KeyValuePair<string, Button> entry in resolutionButtons) {
                entry.Value.Draw(spriteBatch);
            }

            spriteBatch.DrawString(consolasFont, 
            "A - Left, D - Right, W - Jump \n" + 
            "Q - HandGun, E - Rocket Launcher, Space - Shoot \n" + 
            "V -  Place Mine \n" +
            "With the HandGun you hit ground enemies and with the RPG UFOs.", 
            new Vector2(width / 2 - 300, height / 2 - 150), Color.White);

            backButton.Draw(spriteBatch);

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

        private static void DrawPlatforms(SpriteBatch spriteBatch) {
            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].Draw(spriteBatch);
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
                optionsButton.UpdateRectangle(buttonRectangles["options"]);
                backButton.UpdateRectangle(buttonRectangles["back"]);
                replayButton.UpdateRectangle(buttonRectangles["play"]);
                
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