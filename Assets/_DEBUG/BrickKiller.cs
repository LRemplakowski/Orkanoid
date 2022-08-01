using Orkanoid.Level.Bricks;
using SunsetSystems.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.DebugHelper
{
    public class BrickKiller : MonoBehaviour
    {
        public void KillRandomBrick()
        {
            Debug.Log("DIE YOU NASTY BRICK!");
            AbstractBrick brick = this.FindFirstComponentWithTag<AbstractBrick>(TagConstants.BRICK);
            brick.TakeHit(brick.GetHealthLeft());
        }
    }
}
