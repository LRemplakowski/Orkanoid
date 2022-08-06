using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    public static class PatternProvider
    {
        public static async Task<bool[][]> GetNoisePattern(System.Random random, int width, int height, MirrorAxis mirrorAxis, float brickDensity)
        {
            bool[][] result = new bool[height][];
            await Task.Run(() =>
            {
                brickDensity = Mathf.Clamp01(brickDensity);
                for (int i = 0; i < height; i++)
                {
                    result[i] = new bool[width];
                    for (int j = 0; j < width; j++)
                    {
                        float noiseX = (float)random.NextDouble();
                        float noiseY = (float)random.NextDouble();
                        float noisePoint = Mathf.PerlinNoise(noiseX, noiseY);
                        bool cellValue = noisePoint <= brickDensity;
                        result[i][j] = cellValue;
                    }
                }
            });
            return mirrorAxis switch
            {
                MirrorAxis.Y => await MirrorY(result),
                MirrorAxis.X => await MirrorX(result),
                MirrorAxis.XY => await MirrorXY(result),
                MirrorAxis.None => result,
                _ => result,
            };

            async Task<bool[][]> MirrorY(bool[][] pattern)
            {
                bool[][] result = new bool[pattern.Length][];
                await Task.Run(() =>
                {
                    for (int i = 0; i < pattern.Length; i++)
                    {
                        bool[] row = new bool[pattern[i].Length];
                        for (int j = 0; j < (row.Length - row.Length % 2) / 2; j++)
                        {
                            row[j] = pattern[i][j];
                            row[row.Length - 1 - j] = pattern[i][j];
                        }
                        if (row.Length % 2 == 1)
                        {
                            row[(row.Length - 1) / 2] = pattern[i][(row.Length - 1) / 2];
                        }
                        result[i] = row;
                    }
                });
                return result;
            }

            async Task<bool[][]> MirrorX(bool[][] pattern)
            {
                bool[][] result = new bool[pattern.Length][];
                await Task.Run(() =>
                {
                    for (int i = 0; i < (pattern.Length - pattern.Length % 2) / 2; i++)
                    {
                        result[i] = pattern[i];
                        result[pattern.Length - 1 - i] = pattern[i];
                    }
                    if (pattern.Length % 2 == 1)
                    {
                        result[(result.Length - 1) / 2] = pattern[(pattern.Length - 1) / 2];
                    }
                });
                return result;
            }

            async Task<bool[][]> MirrorXY(bool[][] pattern)
            {
                return await MirrorY(await MirrorX(pattern));
            }
        }
    }
}
