using SharpConfig;

namespace Explore
{
    public static class Config
    {
        private static Configuration config;

        public static Section Player {
            get;
            private set;
        }

        public static Section BaseEnemy {
            get;
            private set;
        }

        public static Section BaseShip {
            get;
            private set;
        }

        public static Section Bullet {
            get;
            private set;
        }

        public static Section MapGeneration {
            get;
            private set;
        }

        public static Section Game {
            get;
            private set;
        }

        public static void Load() {
            config = Configuration.LoadFromFile("config.cfg");

            Player = config["Player"];
            BaseEnemy = config["BaseEnemy"];
            BaseShip = config["BaseShip"];
            Bullet = config["Bullets"];
            MapGeneration = config["MapGeneration"];
            Game = config["Game"];
        }
    }
}