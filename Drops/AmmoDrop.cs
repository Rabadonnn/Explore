using Microsoft.Xna.Framework;

namespace Explore.Drops
{
    public class AmmoDrop : Drop {
        private int ammoToGive;

        public AmmoDrop(Vector2 _position) : base(_position) {
            ammoToGive = rand.Next(15, 45);
        }

        public override void SetTexture() {
            texture = GameManager.Assets["ammo_drop"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveHandGunAmmo(ammoToGive);
        }
    }
}