using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Game
{
    public class EmptyBrick : DefaultBrick
    {
        protected override void Awake()
        {
        }

        protected override void OnHealthBelowZero(AbstractBrick brick)
        {
        }

        protected override void OnHitTaken(AbstractBrick brick)
        {
        }

        protected override void Start()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(this.transform.position, spriteRenderer.sprite.bounds.size);
        }
    }
}
