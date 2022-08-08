using Orkanoid.Core;
using SunsetSystems.Utils;

namespace Orkanoid.Game
{
    public class IndestructibleBrick : AbstractBrick
    {
        public sealed override BrickType GetBrickType() => BrickType.Indestructible;

        public override void TakeHit(int damage)
        {
            base.TakeHit(0);
        }

        protected override void OnHitTaken(IBrick brick)
        {
            // Do nothing, this brick shouldn't be destroyed with damage.
        }
    }
}
