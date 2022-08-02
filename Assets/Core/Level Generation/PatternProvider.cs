using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    public static class PatternProvider
    {
        public static int[,] GetNoisePattern(System.Random random, int width, int height, int maxValue, bool useSymmetry, SymmetryAxis symmetryAxis)
        {
            Debug.Log("width " + width + "; height " + height);
            int[,] result = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float noiseX = (float)random.NextDouble();
                    float noiseY = (float)random.NextDouble();
                    float noisePoint = Mathf.PerlinNoise(noiseX, noiseY);
                    int cellValue = Mathf.FloorToInt(Mathf.Clamp((noisePoint * 10f) / maxValue, 0f, maxValue));
                    Debug.Log("noise X = " + noiseX + "; noise Y = " + noiseY + "; noise point " + noisePoint + "; cell value " + cellValue);
                    result[i, j] = cellValue;
                }
            }
            return result;
        }

        public static int[,] GetNoisePattern(System.Random random, int width, int height, int maxValue)
        {
            return GetNoisePattern(random, width, height, maxValue, false, SymmetryAxis.None);
        }
    }
}
