using Orkanoid.Game;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.DebugHelper
{
    public class BrickHitter : MonoBehaviour
    {
        [SerializeField]
        private int damage = 1;

        public void HitRandomBrick()
        {
            Debug.Log("TAKE THAT YOU BRICK!");
            AbstractBrick brick = this.FindFirstComponentWithTag<AbstractBrick>(TagConstants.BRICK);
            if (brick)
                brick.TakeHit(damage);
        }
    }
}