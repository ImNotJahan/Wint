using UnityEngine;

public class Biomes : MonoBehaviour
{
    public static int[,] biomeMap;

    void Start()
    {
        CreateBiomeMap();
    }

    public static int[,] CreateBiomeMap()
    {
        float[,] temperatureMap;
        float[,] humidityMap;

        biomeMap = new int[MeshGenerator.size + 1, MeshGenerator.size + 1];

        temperatureMap = Noise.generateSimple(MeshGenerator.size + 1, MeshGenerator.seed + 809);
        humidityMap = Noise.generateSimple(MeshGenerator.size + 1, MeshGenerator.seed + 831);

        for (int y = 0; y < MeshGenerator.size + 1; y++)
        {
            for (int x = 0; x < MeshGenerator.size + 1; x++)
            {
                if((temperatureMap[x, y] + humidityMap[x, y]) / 2 == 1)
                {
                    Vector2 drunkPos = new Vector2(x, y);

                    float chance = 1;

                    for(int k = 0; k < 50; k++)
                    {
                        if (drunkPos.x > 0)
                        {
                            drunkPos
                        }
                        chance -= 0.02f;
                    }
                }
            }
        }

        return biomeMap;
    }
}
