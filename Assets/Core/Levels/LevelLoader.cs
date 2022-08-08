using Orkanoid.Core.Saves;
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
        private BrickGrid brickGrid;

        internal async Task NextLevel(int seed)
        {
            EnsureDependencies();
            brickGrid.ReturnBricksToPool();
            BrickGrid.ResetBrickCounter();
            await levelGenerator.GenerateLevel(seed);
        }

        internal void LoadSavedLevel(List<BrickData> savedBricks)
        {
            EnsureDependencies();
            brickGrid.ReturnBricksToPool();
            BrickGrid.ResetBrickCounter();
            bool cachedSoundMuted = SoundController.MuteSounds;
            SoundController.MuteSounds = true;
            foreach (BrickData data in savedBricks)
            {
                IBrick brick = brickPool.GetBrickFromPool(data.TemplateID);
                brickGrid.PlaceBrickInGrid(brick, (int)data.Position.x, (int)data.Position.y);
                brick.TakeHit(data.HitsTaken);

            }
            SoundController.MuteSounds = cachedSoundMuted;
        }

        private void EnsureDependencies()
        {
            if (!levelGenerator)
                levelGenerator = this.FindFirstComponentWithTag<LevelGenerator>(TagConstants.LEVEL_GENERATOR);
            if (!brickPool)
                brickPool = this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL);
            if (!brickGrid)
                brickGrid = this.FindFirstComponentWithTag<BrickGrid>(TagConstants.BRICK_GRID);
        }
    }
}
