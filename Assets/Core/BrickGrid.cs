using Orkanoid.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    public class BrickGrid : MonoBehaviour
    {
        [SerializeField]
        private int gridSizeX = 1, gridSizeY = 1;
        [SerializeField]
        private IBrick[,] grid;

        [SerializeField]
        private AbstractBrick emptyBrick;
        [SerializeField]
        private Grid myGridComponent;

        private void Awake()
        {
            grid = new IBrick[gridSizeX, gridSizeY];
            myGridComponent = GetComponent<Grid>();
        }

        [ContextMenu("Populate grid empty")]
        private void PopulateGridEmpty()
        {
            this.transform.DestroyChildrenImmediate();
            grid = new IBrick[gridSizeX, gridSizeY];
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    PlaceBrickInGrid(Instantiate(emptyBrick), i, j);
                }
            }
        }

        public void PlaceBrickInGrid(AbstractBrick brick, int x, int y)
        {
            if (grid == null)
                grid = new IBrick[gridSizeX, gridSizeY];
            try
            {
                grid[x, y] = brick;
                brick.transform.parent = this.transform;
                brick.transform.localPosition = myGridComponent.GetCellCenterLocal(new Vector3Int(x, y));
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogException(e);
                Destroy(brick);
            }
        }

        private void OnDrawGizmos()
        {

        }
    }
}
