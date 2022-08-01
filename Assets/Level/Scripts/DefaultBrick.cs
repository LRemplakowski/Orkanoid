using System;
using UnityEngine;

namespace Orkanoid.Level.Bricks
{
    public class DefaultBrick : AbstractBrick
    {
        [SerializeField]
        protected AudioClip brickHit;

        protected override void OnHealthBelowZero(AbstractBrick brick)
        {
            Destroy(brick.gameObject);
        }

        protected override void OnHitTaken(AbstractBrick brick)
        {
            try
            {
                spriteRenderer.sprite = sprites[hitsTaken < maxHealth ? hitsTaken : maxHealth];
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
            }
            if (brickHit)
                AudioSource.PlayClipAtPoint(brickHit, transform.position, 1.0f);
        }
    }
}
