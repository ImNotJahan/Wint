using UnityEngine;

public static class Noise
{
    public static float[] Generate(int mapSize, int seed, float initialScale, int numOctaves, 
        float persistence, float lacunarity, Vector2 offset)
    {
        float[] map = new float[mapSize * mapSize];
        System.Random prng = new System.Random(seed);

        Vector2[] offsets = new Vector2[numOctaves];
        for (int i = 0; i < numOctaves; i++)
        {
            offsets[i] = new Vector2(prng.Next(-1000, 1000) + offset.x, prng.Next(-1000, 1000) + offset.y);
        }

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float noiseValue = 0;
                float scale = initialScale;
                float weight = 1;
                for (int i = 0; i < numOctaves; i++)
                {
                    Vector2 p = new Vector2((x + offsets[i].x) / mapSize * scale, 
                        (y + offsets[i].y) / mapSize * scale);
                    
                    noiseValue += Mathf.PerlinNoise(p.x, p.y) * weight;

                    weight *= persistence;
                    scale *= lacunarity;
                }
                map[y * mapSize + x] = noiseValue;
                minValue = Mathf.Min(noiseValue, minValue);
                maxValue = Mathf.Max(noiseValue, maxValue);
            }
        }

        // Normalize
        if (maxValue != minValue)
        {
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = (map[i] - minValue) / (maxValue - minValue);
            }
        }

        return map;
    }
}
