using Orkanoid.Core.Levels;
using Orkanoid.Game;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.DebugHelper
{
    public class BrickKiller : MonoBehaviour
    {
        public void KillRandomBrick()
        {
            Debug.Log("DIE YOU NASTY BRICK!");
            BrickGrid grid = this.FindFirstComponentWithTag<BrickGrid>(TagConstants.BRICK_GRID);
            IBrick brick = grid.GetRandomBrick();
            brick.TakeHit(brick.GetHealthLeft() * 2);
        }

        public void KillAllBricks()
        {
            Debug.Log("Bender would be proud!");
            BrickGrid grid = this.FindFirstComponentWithTag<BrickGrid>(TagConstants.BRICK_GRID);
            grid.ReturnBricksToPool(true);
        }
    }
}
