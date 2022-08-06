using Orkanoid.Game;
using Orkanoid.UI;
using SunsetSystems.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    [RequireComponent(typeof(Tagger))]
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private LevelGenerator levelGenerator;
        [SerializeField]
        private BrickPool brickPool;
        [SerializeField]
        private FadePanel fadePanel;

        public async Task NextLevel(int seed)
        {
            EnsureDependencies();
            ClearPreviousLevel();
            await levelGenerator.GenerateLevel(seed);

            void ClearPreviousLevel()
            {
                List<AbstractBrick> bricks = this.FindAllComponentsWithTag<AbstractBrick>(TagConstants.BRICK);
                foreach (IBrick brick in bricks)
                {
                    brickPool.ReturnToPool(brick);
                }
            }

            void EnsureDependencies()
            {
                if (!levelGenerator)
                    levelGenerator = this.FindFirstComponentWithTag<LevelGenerator>(TagConstants.LEVEL_GENERATOR);
                if (!brickPool)
                    brickPool = this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL);
                if (!fadePanel)
                    fadePanel = this.FindFirstComponentWithTag<FadePanel>(TagConstants.FADE_PANEL);
            }
        }
    }
}
