using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    public class ResizePaddle : AbstractPowerUp
    {
        [SerializeField]
        private PaddleAdjust paddleAdjust;

        public override void Apply()
        {
            Paddle paddle = this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE);
            switch (paddleAdjust)
            {
                case PaddleAdjust.IncreaseWidth:
                    paddle.IncreasePaddleWidth();
                    break;
                case PaddleAdjust.DecreaseWidth:
                    paddle.DecreasePaddleWidth();
                    break;
            }
        }

        public enum PaddleAdjust
        {
            IncreaseWidth, DecreaseWidth
        }
    }
}
