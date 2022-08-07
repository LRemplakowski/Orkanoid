using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    public class ResizePaddle : AbstractPowerUp
    {
        [SerializeField]
        private float sizeMultiplier;

        public override void Apply()
        {
            Paddle paddle = this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE);
            paddle.ResizePaddle(sizeMultiplier);
        }
    }
}
