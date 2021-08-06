using UnityEngine;

public static class Voronoi
{
    public static int size = 201;
    public static int regionAmount = 20;
    public static bool drawByDistance = false;

    public static float[,] Generate()
    {
        Vector2Int[] centroids = new Vector2Int[regionAmount];

        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, size), Random.Range(0, size));
        }
        float[] distances = new float[size * size];

        float maxDst = float.MinValue;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int index = x * size + y;
                distances[index] = Vector2.Distance(new Vector2Int(x, y), centroids[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)]);
                if (distances[index] > maxDst)
                {
                    maxDst = distances[index];
                }
            }
        }

        float[,] diagram = new float[size, size];

        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                diagram[x, y] = distances[y * size + x] / maxDst;
            }
        }

        return diagram;
    }

    static int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
    {
        float smallestDst = float.MaxValue;
        int index = 0;
        for (int i = 0; i < centroids.Length; i++)
        {
            if (Vector2.Distance(pixelPos, centroids[i]) < smallestDst)
            {
                smallestDst = Vector2.Distance(pixelPos, centroids[i]);
                index = i;
            }
        }
        return index;
    }
}
