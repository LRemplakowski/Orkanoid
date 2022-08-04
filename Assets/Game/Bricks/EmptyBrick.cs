using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Game
{
    public class EmptyBrick : AbstractBrick
    {
        protected override void Awake()
        {
        }

        protected override void OnHealthBelowZero(IBrick brick)
        {
        }

        protected override void OnHitTaken(IBrick brick)
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

        public sealed override BrickType GetBrickType() => BrickType.Empty;
    }
}
