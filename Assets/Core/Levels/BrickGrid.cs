using Orkanoid.Game;
using SunsetSystems.Utils;
using System;
using UnityEngine;
using System.Linq;

namespace Orkanoid.Core.Levels
{
    [RequireComponent(typeof(Tagger))]
    public class BrickGrid : MonoBehaviour
    {
        [SerializeField]
        private int _gridSizeX = 1, _gridSizeY = 1;
        public int GridWidth => _gridSizeX;
        public int GridHeight => _gridSizeY;
        [SerializeField]
        private IBrick[,] grid;
        [SerializeField]
        private BrickPool brickPool;
        [SerializeField]
        private AbstractBrick emptyBrick;
        [SerializeField]
        private Grid myGridComponent;

        private int brickCount = 0;

        public delegate void AllBricksDestroyedHandler();
        public static event AllBricksDestroyedHandler AllBricksDestroyed;

        private void Awake()
        {
            grid = new IBrick[_gridSizeX, _gridSizeY];
            myGridComponent = GetComponent<Grid>();
        }

        private void Start()
        {
            if (!brickPool)
                brickPool = this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL);
        }

        [ContextMenu("Populate grid empty")]
        private void PopulateGridEmpty()
        {
            this.transform.DestroyChildrenImmediate();
            grid = new IBrick[_gridSizeX, _gridSizeY];
            for (int i = 0; i < _gridSizeX; i++)
            {
                for (int j = 0; j < _gridSizeY; j++)
                {
                    PlaceBrickInGrid(Instantiate(emptyBrick), i, j);
                }
            }
        }

        public IBrick GetRandomBrick()
        {
            foreach (IBrick brick in grid)
            {
                BrickType bt = brick.GetBrickType();
                if (bt.Equals(BrickType.Default) || bt.Equals(BrickType.Special))
                    return brick;
            }
            return null;
        }

        public void PlaceBrickInGrid(IBrick brick, int x, int y)
        {
            if (grid == null)
                grid = new IBrick[_gridSizeX, _gridSizeY];
            try
            {
                if (grid[x, y] != null)
                    brickPool.ReturnToPool(grid[x, y]);
                grid[x, y] = brick;
                Vector3 worldPosition = myGridComponent.GetCellCenterWorld(new Vector3Int(x, y));
                worldPosition.z = -5;
                brick.GetTransform().localPosition = worldPosition;
                if (brick is AbstractBrick b)
                {
                    if (!b.GetBrickType().Equals(BrickType.Empty) && !b.GetBrickType().Equals(BrickType.Indestructible))
                        brickCount++;
                    b.BrickDestroyed += OnBrickDestroyed;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
                brickPool.ReturnToPool(brick);
            }

            void OnBrickDestroyed(AbstractBrick brick)
            {
                PlaceBrickInGrid(brickPool.GetBrickFromPool(emptyBrick), x, y);
                if (!brick.GetBrickType().Equals(BrickType.Empty) && !brick.GetBrickType().Equals(BrickType.Indestructible))
                    brickCount--;
                if (brickCount <= 0)
                {
                    AllBricksDestroyed?.Invoke();
                }
                brick.BrickDestroyed -= OnBrickDestroyed;
            }
        }
    }
}
