using System;
using UnityEngine;

namespace Orkanoid.Game
{
    public class DefaultBrick : AbstractBrick
    {
        [SerializeField]
        protected AudioClip brickHit;

        protected override void OnHealthBelowZero(IBrick brick)
        {
            gameManager.AddPoints(brick.GetPointValue());
            brickPool.ReturnToPool(brick);
        }

        protected override void OnHitTaken(IBrick brick)
        {
            try
            {
                spriteRenderer.sprite = sprites[hitsTaken < maxHealth - 1 ? hitsTaken : maxHealth - 1];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
            }
            if (brickHit)
                AudioSource.PlayClipAtPoint(brickHit, transform.position, 1.0f);
        }

        public sealed override BrickType GetBrickType() => BrickType.Default;
    }
}
