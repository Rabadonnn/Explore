using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Explore
{
    public static class SoundManager
    {
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public static void LoadSounds(ContentManager contentManager) {
            sounds.Add("shoot", contentManager.Load<SoundEffect>("Shoot"));
            sounds.Add("rocketShoot", contentManager.Load<SoundEffect>("RocketShoot"));
        }

        public static void Shoot() {
            sounds["shoot"].CreateInstance().Play();
        }

        public static void LaunchRocket() {
            sounds["rocketShoot"].CreateInstance().Play();
        }
    }
}