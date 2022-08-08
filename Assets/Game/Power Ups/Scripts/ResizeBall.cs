using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    public class ResizeBall : AbstractPowerUp
    {
        [SerializeField]
        private BallAdjust adjustBall;

        public override void Apply()
        {
            switch (adjustBall)
            {
                case BallAdjust.IncreaseRadius:
                    Ball.IncreaseBallSize();
                    break;
                case BallAdjust.DecreaseRadius:
                    Ball.DecreaseBallSize();
                    break;
            }
            Ball.AdjustBallDamage();
        }

        public enum BallAdjust
        {
            IncreaseRadius, DecreaseRadius
        }
    }
}
