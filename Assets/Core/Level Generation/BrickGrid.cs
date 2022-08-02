using Orkanoid.Game;
using SunsetSystems.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    [RequireComponent(typeof(Tagger))]
    public class BrickGrid : MonoBehaviour
    {
        [SerializeField]
        private int _gridSizeX = 1, gridSizeY = 1;
        public int GridWidth => _gridSizeX;
        public int GridHeight => gridSizeY;
        [SerializeField]
        private IBrick[,] grid;

        [SerializeField]
        private AbstractBrick emptyBrick;
        [SerializeField]
        private Grid myGridComponent;

        private void Awake()
        {
            grid = new IBrick[_gridSizeX, gridSizeY];
            myGridComponent = GetComponent<Grid>();
        }

        [ContextMenu("Populate grid empty")]
        private void PopulateGridEmpty()
        {
            this.transform.DestroyChildrenImmediate();
            grid = new IBrick[_gridSizeX, gridSizeY];
            for (int i = 0; i < _gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    PlaceBrickInGrid(Instantiate(emptyBrick), i, j);
                }
            }
        }

        public void PlaceBrickInGrid(IBrick brick, int x, int y)
        {
            if (grid == null)
                grid = new IBrick[_gridSizeX, gridSizeY];
            try
            {
                if (grid[x, y] != null)
                    Destroy(grid[x, y].GetGameObject());
                grid[x, y] = brick;
                brick.GetTransform().parent = this.transform;
                Vector3 localPosition = myGridComponent.GetCellCenterLocal(new Vector3Int(x, y));
                localPosition.z = -5;
                brick.GetTransform().localPosition = localPosition;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
                Destroy(brick.GetGameObject());
            }
        }

        private void OnDrawGizmos()
        {

        }
    }
}
