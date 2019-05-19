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
            sounds.Add("shipExplosion", contentManager.Load<SoundEffect>("ShipExplosion"));
            sounds.Add("gotHit", contentManager.Load<SoundEffect>("gotHit"));
        }

        public static void Shoot() {
            sounds["shoot"].CreateInstance().Play();
        }

        public static void LaunchRocket() {
            sounds["rocketShoot"].CreateInstance().Play();
        }

        public static void ShipExplosion() {
            sounds["shipExplosion"].CreateInstance().Play();
        }

        public static void GotHit() {
            sounds["gotHit"].CreateInstance().Play();
        }
    }
}