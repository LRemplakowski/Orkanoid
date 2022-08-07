using Orkanoid.Core;
using SunsetSystems.Utils;

namespace Orkanoid.Game
{
    public class IndestructibleBrick : AbstractBrick
    {
        public sealed override BrickType GetBrickType() => BrickType.Indestructible;

        public override void TakeHit(int damage)
        {

        }

        // This shouldn't ever be called for an indestructible brick, but we'll return it to the object pool just in case.
        protected override void OnHealthBelowZero(IBrick brick)
        {
            this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL).ReturnToPool(brick);
        }

        // This shouldn't ever be called for an indestructible brick, but we'll return it to the object pool just in case.
        protected override void OnHitTaken(IBrick brick)
        {
            this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL).ReturnToPool(brick);
        }
    }
}
