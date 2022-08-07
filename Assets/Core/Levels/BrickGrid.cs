using Orkanoid.Game;
using SunsetSystems.Utils;
using System;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    [RequireComponent(typeof(Tagger))]
    public class BrickGrid : MonoBehaviour
    {
        [SerializeField]
        private int _gridSizeX = 1, _gridSizeY = 1;
        public int GridWidth => _gridSizeX;
        public int GridHeight => _gridSizeY;
        private static IBrick[,] _grid;
        public static IBrick[,] Grid { get => _grid; }
        [SerializeField]
        private BrickPool brickPool;
        [SerializeField]
        private AbstractBrick emptyBrick;
        [SerializeField]
        private Grid myGridComponent;

        private void Awake()
        {
            _grid = new IBrick[_gridSizeX, _gridSizeY];
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
            _grid = new IBrick[_gridSizeX, _gridSizeY];
            for (int i = 0; i < _gridSizeX; i++)
            {
                for (int j = 0; j < _gridSizeY; j++)
                {
                    PlaceBrickInGrid(Instantiate(emptyBrick), i, j);
                }
            }
        }

        public void ReturnBricksToPool()
        {
            foreach (IBrick brick in _grid)
            {
                if (brick != null)
                {
                    brickPool.ReturnToPool(brick);
                    RemoveBrickFromGrid(brick.GetGridPosition().x, brick.GetGridPosition().y);
                }
            }
        }

        public IBrick GetRandomBrick()
        {
            foreach (IBrick brick in _grid)
            {
                if (brick == null)
                    continue;
                BrickType bt = brick.GetBrickType();
                if ((bt.Equals(BrickType.Default) || bt.Equals(BrickType.Special)) && brick.GetGameObject().activeSelf)
                    return brick;
            }
            return null;
        }

        public void PlaceBrickInGrid(IBrick brick, int x, int y)
        {
            if (_grid == null)
                _grid = new IBrick[_gridSizeX, _gridSizeY];
            if (brick == null)
                return;
            try
            {
                if (_grid[x, y] != null && !brick.Equals(_grid[x, y]))
                    brickPool.ReturnToPool(_grid[x, y]);
                _grid[x, y] = brick;
                brick.SetGridPosition(x, y);
                brick.GetTransform().parent = transform;
                Vector3 localPosition = myGridComponent.GetCellCenterLocal(new Vector3Int(x, y));
                localPosition.z = -5;
                brick.GetTransform().localPosition = localPosition;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
                brickPool.ReturnToPool(brick);
            }
        }

        public void RemoveBrickFromGrid(int x, int y)
        {
            _grid[x, y] = null;
        }
    }
}
