using Orkanoid.Game;
using SunsetSystems.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    [RequireComponent(typeof(Tagger))]
    public class LevelGenerator : MonoBehaviour
    {
        private System.Random random;
        private BrickGrid brickGrid;

        [SerializeField]
        private int seed = 1;

        [SerializeField]
        private List<AbstractBrick> brickTemplates = new();

        // Start is called before the first frame update
        private void Start()
        {
            random = new System.Random(seed);
            brickGrid = this.FindFirstComponentWithTag<BrickGrid>(TagConstants.BRICK_GRID);
            int[,] levelPattern = PatternProvider.GetNoisePattern(random, brickGrid.GridWidth, brickGrid.GridHeight, brickTemplates.Count - 1);
            for (int i = 0; i < brickGrid.GridWidth; i++)
            {
                for (int j = 0; j < brickGrid.GridHeight; j++)
                {
                    brickGrid.PlaceBrickInGrid(Instantiate(brickTemplates[levelPattern[i, j]]), i, j);
                }
            }
        }
    }
}
