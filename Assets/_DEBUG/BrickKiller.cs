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
            AbstractBrick brick = this.FindFirstComponentWithTag<AbstractBrick>(TagConstants.BRICK);
            if (brick)
                brick.TakeHit(brick.GetHealthLeft());
        }
    }
}
