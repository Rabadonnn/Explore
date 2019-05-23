using Microsoft.Xna.Framework;

namespace Explore.Drops
{
    public class MinesDrop : Drop {
        private int minesToGive;
        
        public MinesDrop(Vector2 _position) : base(_position) {
            minesToGive = rand.Next(2, 5);
        }

        public override void SetTexture() {
            texture = GameManager.Assets["mines_drop"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveMines(minesToGive);
        }
    }
}