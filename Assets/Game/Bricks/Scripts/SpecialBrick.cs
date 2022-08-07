using Orkanoid.Core;
using UnityEngine;

namespace Orkanoid.Game
{
    public class SpecialBrick : AbstractBrick
    {
        [SerializeField]
        protected AudioClip brickHit;
        [SerializeField]
        protected AbstractPowerUp powerUp;

        protected override bool CountsTowardsWin => true;
        public sealed override BrickType GetBrickType() => BrickType.Special;

        public override int GetPointValue()
        {
            return base.GetPointValue() * 2;
        }

        protected override void OnHealthBelowZero(IBrick brick)
        {
            AbstractPowerUp powerUpInstance = Instantiate(powerUp);
            powerUpInstance.transform.position = brick.GetTransform().position;
            gameManager.AddPoints(brick.GetPointValue());
            brickPool.ReturnToPool(brick);
        }

        protected override void OnHitTaken(IBrick brick)
        {
            if (brickHit && !SoundController.MuteSounds)
                AudioSource.PlayClipAtPoint(brickHit, brick.GetTransform().position, 1.0f);
        }
    }
}
