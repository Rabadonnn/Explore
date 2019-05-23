using Microsoft.Xna.Framework;

namespace Explore.Drops
{
    public class RocketsDrop : Drop {
        private int rocketsToGive;
        
        public RocketsDrop(Vector2 _position) : base(_position) {
            rocketsToGive = rand.Next(4, 6);
        }

        public override void SetTexture() {
            texture = GameManager.Assets["rockets_drop"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveRockets(rocketsToGive);
        }
    }
}