using Orkanoid.Game;
using SunsetSystems.Utils;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        private MirrorAxis mirrorAxis = MirrorAxis.None;
        [SerializeField]
        private bool randomizeLevelSymmetry = false;
        [SerializeField, Range(0f, 1f)]
        private float brickDensity;

        [SerializeField]
        private List<AbstractBrick> brickTemplates = new();
        [SerializeField]
        private AbstractBrick emptyBrick;

        // Start is called before the first frame update
        private void Start()
        {
            random = new System.Random(seed);
            brickGrid = this.FindFirstComponentWithTag<BrickGrid>(TagConstants.BRICK_GRID);
            BrickPool brickPool = this.FindFirstComponentWithTag<BrickPool>(TagConstants.BRICK_POOL);
            bool[][] levelPattern = new bool[brickGrid.GridHeight][];
            levelPattern = BuildLevelPatternRecursive(levelPattern, brickDensity);
            GridTemplate gridTemplate = new GridTemplate.GridTemplateBuilder(levelPattern, new(brickTemplates), emptyBrick, random, brickPool)
                .SetSymmetryAxis(mirrorAxis)
                .Build();
            for (int i = 0; i < brickGrid.GridWidth; i++)
            {
                for (int j = 0; j < brickGrid.GridHeight; j++)
                {
                    IBrick brick = gridTemplate.Get(i, j);
                    Debug.Log("Brick type at coordinates [" + i + ", " + j + "] = " + brick.GetBrickType().ToString());
                    brickGrid.PlaceBrickInGrid(gridTemplate.Get(i, j), i, j);
                }
            }
        }

        private bool[][] BuildLevelPatternRecursive(bool[][] levelPattern, float density)
        {
            levelPattern = PatternProvider.GetNoisePattern(random, brickGrid.GridWidth, brickGrid.GridHeight, mirrorAxis, density);
            if (levelPattern.All(rows => rows.All(fields => fields == false)))
            {
                return BuildLevelPatternRecursive(levelPattern, density + 0.1f);
            }
            else
            {
                return levelPattern;
            }
        }

        private class GridTemplate
        {
            private readonly BrickRow[] _rows;

            private GridTemplate(BrickRow[] _rows)
            {
                this._rows = _rows;
            }

            public IBrick Get(int x, int y)
            {
                return _rows[y].Bricks[x];
            }

            public class GridTemplateBuilder
            {
                private bool[][] perlinPattern;
                private List<IBrick> brickTemplates;
                private IBrick emptyBrick;
                private MirrorAxis mirrorAxis = MirrorAxis.None;

                private System.Random seededRandom;
                private BrickPool brickPool;

                public GridTemplateBuilder(bool[][] perlinPattern, List<IBrick> brickTemplates, IBrick emptyBrick, System.Random seededRandom, BrickPool brickPool)
                {
                    this.perlinPattern = perlinPattern;
                    this.brickTemplates = brickTemplates;
                    this.emptyBrick = emptyBrick;
                    this.seededRandom = seededRandom;
                    this.brickPool = brickPool;
                }

                public GridTemplateBuilder SetRowPattern(bool[][] perlinPattern)
                {
                    this.perlinPattern = perlinPattern;
                    return this;
                }

                public GridTemplateBuilder SetBrickTemplates(List<IBrick> brickTemplates)
                {
                    this.brickTemplates = brickTemplates;
                    return this;
                }

                public GridTemplateBuilder SetEmptyBrick(AbstractBrick emptyBrick)
                {
                    this.emptyBrick = emptyBrick;
                    return this;
                }

                public GridTemplateBuilder SetSymmetryAxis(MirrorAxis symmetryAxis)
                {
                    this.mirrorAxis = symmetryAxis;
                    return this;
                }

                public GridTemplateBuilder SetSeededRandom(System.Random seededRandom)
                {
                    this.seededRandom = seededRandom;
                    return this;
                }

                public GridTemplateBuilder SetPool(BrickPool brickPool)
                {
                    this.brickPool = brickPool;
                    return this;
                }

                public GridTemplate Build()
                {
                    BrickRow[] rows = new BrickRow[perlinPattern.Length];
                    BrickRow.BrickRowBuilder rowBuilder = new(perlinPattern[0],
                                          brickTemplates,
                                          emptyBrick,
                                          seededRandom,
                                          brickPool);
                    rowBuilder.SetMirrorAxis(mirrorAxis);
                    if (mirrorAxis.Equals(MirrorAxis.X) || mirrorAxis.Equals(MirrorAxis.XY))
                    {
                        for (int i = 0; i < (perlinPattern.Length - perlinPattern.Length % 2) / 2; i++)
                        {
                            BrickRow row = rowBuilder
                                .SetRowPattern(perlinPattern[i])
                                .Build();
                            rows[i] = row;
                            rows[rows.Length - 1 - i] = new(row, brickPool);
                        }
                        if (perlinPattern.Length % 2 == 1)
                        {
                            int middleColumnIndex = (perlinPattern.Length - 1) / 2;
                            rows[middleColumnIndex] = rowBuilder
                                .SetRowPattern(perlinPattern[middleColumnIndex])
                                .Build();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < perlinPattern.Length; i++)
                        {
                            rows[i] = rowBuilder
                                .SetRowPattern(perlinPattern[i])
                                .Build();
                        }
                    }
                    return new GridTemplate(rows);
                }
            }

            private class BrickRow
            {
                private readonly IBrick[] _bricks;
                public IBrick[] Bricks => _bricks;

                private BrickRow(IBrick[] _bricks)
                {
                    this._bricks = _bricks;
                }

                public BrickRow(BrickRow row, BrickPool brickPool)
                {
                    _bricks = new IBrick[row.Bricks.Length];
                    for (int i = 0; i < _bricks.Length; i++)
                    {
                        _bricks[i] = brickPool.GetBrickFromPool(row.Bricks[i]);
                    }
                }

                public class BrickRowBuilder
                {
                    private bool[] rowPattern;
                    private List<IBrick> brickTemplates;
                    private IBrick emptyBrick;
                    private MirrorAxis mirrorAxis = MirrorAxis.None;

                    private System.Random seededRandom;
                    private BrickPool brickPool;

                    public BrickRowBuilder(bool[] rowPattern, List<IBrick> brickTemplates, IBrick emptyBrick, System.Random seededRandom, BrickPool brickPool)
                    {
                        this.rowPattern = rowPattern;
                        this.brickTemplates = brickTemplates;
                        this.emptyBrick = emptyBrick;
                        this.seededRandom = seededRandom;
                        this.brickPool = brickPool;
                    }

                    public BrickRowBuilder SetRowPattern(bool[] rowPattern)
                    {
                        this.rowPattern = rowPattern;
                        return this;
                    }

                    public BrickRowBuilder SetBrickTemplates(List<IBrick> brickTemplates)
                    {
                        this.brickTemplates = brickTemplates;
                        return this;
                    }

                    public BrickRowBuilder SetEmptyBrick(AbstractBrick emptyBrick)
                    {
                        this.emptyBrick = emptyBrick;
                        return this;
                    }

                    public BrickRowBuilder SetMirrorAxis(MirrorAxis mirrorAxis)
                    {
                        this.mirrorAxis = mirrorAxis;
                        return this;
                    }

                    public BrickRowBuilder SetSeededRandom(System.Random seededRandom)
                    {
                        this.seededRandom = seededRandom;
                        return this;
                    }

                    public BrickRowBuilder SetPool(BrickPool brickPool)
                    {
                        this.brickPool = brickPool;
                        return this;
                    }

                    public BrickRow Build()
                    {
                        IBrick[] bricks = new IBrick[rowPattern.Length];
                        if (mirrorAxis.Equals(MirrorAxis.Y) || mirrorAxis.Equals(MirrorAxis.XY))
                        {
                            for (int i = 0; i < (bricks.Length - bricks.Length % 2) / 2; i++)
                            {
                                IBrick template = rowPattern[i] ? brickTemplates[seededRandom.Next(0, brickTemplates.Count)] : emptyBrick;
                                bricks[i] = GetBrick(template);
                                bricks[bricks.Length - 1 - i] = GetBrick(template);
                            }
                            if (bricks.Length % 2 == 1)
                            {
                                IBrick template = rowPattern[(rowPattern.Length - 1) / 2] ? brickTemplates[seededRandom.Next(0, brickTemplates.Count)] : emptyBrick;
                                bricks[(bricks.Length - 1) / 2] = GetBrick(template);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < bricks.Length; i++)
                            {
                                IBrick template = rowPattern[i] ? brickTemplates[seededRandom.Next(0, brickTemplates.Count)] : emptyBrick;
                                bricks[i] = GetBrick(template);
                            }
                        }
                        return new BrickRow(bricks);

                        IBrick GetBrick(IBrick template)
                        {
                            return brickPool.GetBrickFromPool(template);
                        }
                    }
                }
            }
        }
    }
}
