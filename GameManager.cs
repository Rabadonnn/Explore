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
        public static int Width = 1600;
        public static int Height = 900;

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

        public static Player player;

        public static List<Platform> platforms;
        public static List<Enemy> enemies = new List<Enemy>();

        private static List<DropShip> dropShips = new List<DropShip>() {
            new DropShip(new Vector2(-100, 0)),
            new DropShip(new Vector2(100, 0))
        };

        public static void Initialize() {
            assets = new Dictionary<string, Texture2D>();
            player = new Player();
            platforms = new List<Platform>() {
                new Platform(new Vector2(0, 400), new Vector2(1200, 15))
            };
        }

        public static void LoadAssets(ContentManager contentManager) {
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
        }

        public static void SetTextures() {
            player.SetAnimations();
            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].SetTexture(assets["square"]);
            }

            for (int i = 0; i < dropShips.Count; i++) {
                dropShips[i].SetTexture(assets["dropship"]);
            }
        }

        public static void Update(GameTime _gameTime) {
            gameTime = _gameTime;
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            GameObject.UpdateList();
            player.Update();

            foreach (DropShip ship in dropShips) {
                ship.Update();
            }

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].Update(player.Position);

                if (enemies[i].isDead) {
                    enemies.RemoveAt(i);
                }
            }
        } 

        public static void Draw(SpriteBatch spriteBatch) {
            DrawBackground(spriteBatch);
            spriteBatch.Begin(Game1.camera, samplerState: SamplerState.PointClamp);

            player.Draw(spriteBatch);

            for (int i = 0; i < platforms.Count; i++) {
                platforms[i].Draw(spriteBatch);
            }

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].Draw(spriteBatch);
            }

            foreach (DropShip ship in dropShips) {
                ship.Draw(spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Draw(Game1.camera.Debug);
            DrawCrosshair(spriteBatch);
        }

        private static void DrawCrosshair(SpriteBatch spriteBatch) {
            Rectangle crosshairRectangle = new Rectangle((int)Input.MouseX - 16, (int)Input.MouseY - 16, 32, 32);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(assets["crosshair"], destinationRectangle: crosshairRectangle, color: Color.White);
            spriteBatch.End();
        }

        private static void DrawBackground(SpriteBatch spriteBatch) {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(assets["background_variant"], destinationRectangle: new Rectangle(0, 0, Width, Height), color: Color.White);
            spriteBatch.End();
        }

        public static void Print(object obj) {
            System.Diagnostics.Debug.WriteLine(obj);
        }
    }
}

//dotnet publish -c release -r win10-x64