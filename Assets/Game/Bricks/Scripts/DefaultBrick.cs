using Orkanoid.Core;
using UnityEngine;

namespace Orkanoid.Game
{
    public class DefaultBrick : AbstractBrick
    {
        [SerializeField]
        protected AudioClip brickHit;

        public override bool CountsTowardsWin => true;
        public sealed override BrickType GetBrickType() => BrickType.Default;

        protected override void OnHealthBelowZero(IBrick brick)
        {
            base.OnHealthBelowZero(brick);
            gameManager.AddPoints(brick.GetPointValue());
        }

        protected override void OnHitTaken(IBrick brick)
        {
            if (brickHit && !SoundController.MuteSounds)
                AudioSource.PlayClipAtPoint(brickHit, transform.position, 1.0f);
        }
    }
}
