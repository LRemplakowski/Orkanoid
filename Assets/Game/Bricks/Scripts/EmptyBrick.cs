using Orkanoid.Core;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    public class EmptyBrick : AbstractBrick
    {
        protected override void Awake()
        {

        }

        // This shouldn't ever be called for an empty brick, but we'll remove it from grid just in case.
        protected override void OnHitTaken(IBrick brick)
        {
            brickGrid.RemoveBrickFromGrid(brick);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(this.transform.position, spriteRenderer.sprite.bounds.size);
        }

        public sealed override BrickType GetBrickType() => BrickType.Empty;
    }
}
