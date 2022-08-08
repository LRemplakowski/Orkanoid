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

        public delegate void AllBricksDestroyedHandler();
        public static event AllBricksDestroyedHandler AllBricksDestroyed;

        public static int BrickCounter { get; private set; }

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

        public static void ResetBrickCounter()
        {
            BrickCounter = 0;
        }

        public void ReturnBricksToPool(bool invokeEvents = false)
        {
            foreach (IBrick brick in _grid)
            {
                if (brick != null)
                {
                    RemoveBrickFromGrid(brick, invokeEvents);
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
                if (brick.CountsTowardsWin)
                    BrickCounter++;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogException(e);
                brickPool.ReturnToPool(brick);
            }
        }

        public void RemoveBrickFromGrid(IBrick brick, bool invokeEvents = true)
        {
            Vector2Int brickPosition = brick.GetGridPosition();
            try
            {
                IBrick brickAtPosition = _grid[brickPosition.x, brickPosition.y];
                if (_grid[brickPosition.x, brickPosition.y].Equals(brick))
                {
                    _grid[brickPosition.x, brickPosition.y] = null;
                    brickPool.ReturnToPool(brick);
                    if (brick.CountsTowardsWin)
                    {
                        BrickCounter--;
                        if (invokeEvents && BrickCounter <= 0)
                            AllBricksDestroyed?.Invoke();
                    }
                }
                else
                {
                    brickPool.ReturnToPool(brick);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogException(e);
                brickPool.ReturnToPool(brick);
            }
        }
    }
}
