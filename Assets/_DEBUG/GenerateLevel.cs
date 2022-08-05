using Orkanoid.Core;
using SunsetSystems.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.DebugHelper
{
    public class GenerateLevel : MonoBehaviour
    {
        public async void GenerateNewLevel(int seed)
        {
            await this.FindFirstComponentWithTag<GameManager>(TagConstants.GAME_MANAGER).NextLevel(seed);
        }
    }
}
