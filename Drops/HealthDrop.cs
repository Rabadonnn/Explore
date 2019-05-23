using Microsoft.Xna.Framework;

namespace Explore.Drops
{
    public class HealthDrop : Drop {
        private int healthToGive;

        public HealthDrop(Vector2 _position) : base(_position) {
            healthToGive = rand.Next(3, 6);
        }
        
        public override void SetTexture() {
            texture = GameManager.Assets["health_drop"];
        }

        protected override void OnPlayerPickup() {
            GameManager.player.GiveHealth(healthToGive);
        }
    }
}