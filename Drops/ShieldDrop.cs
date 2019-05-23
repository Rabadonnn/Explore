using Microsoft.Xna.Framework;

namespace Explore.Drops
{
    public class ShieldDrop : Drop {

        public ShieldDrop(Vector2 _position) : base(_position) {
            width = 32;
            height = 32;
        }

        public override void SetTexture() {
            texture = GameManager.Assets["shield"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveShield();
        }
    }
}