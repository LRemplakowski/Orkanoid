using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Game
{
    public class ResizeBall : AbstractPowerUp
    {
        [SerializeField]
        private float sizeMultiplier = 1f;
        [SerializeField]
        private int ballDamageModifier = 0;

        public override void Apply()
        {
            Ball.ResizeBalls(sizeMultiplier);
            Ball.AdjustBallDamage(ballDamageModifier);
            Paddle paddle = this.FindFirstComponentWithTag<Paddle>(TagConstants.PADDLE);
            paddle.AdjustHookPointPosition(sizeMultiplier);
        }
    }
}
