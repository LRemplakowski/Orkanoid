using Orkanoid.Core;
using SunsetSystems.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Game
{
    public class HealthUp : AbstractPowerUp
    {
        [SerializeField]
        private int livesAdded = 1;

        public override void Apply()
        {
            GameManager gameManager = this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER);
            gameManager.AddLives(livesAdded);
        }
    }
}
