﻿using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] bool shouldPlace = false;
    [SerializeField] bool shouldErode = false;

    [SerializeField] float heightMultiplier = 1;
    [SerializeField] int size = 100;
    [SerializeField] int seed = 42;
    [SerializeField] float scale = 1;
    [SerializeField] float biomeScale = 1;
    [SerializeField] int octaves = 1;
    [SerializeField] float persistance = 1;
    [SerializeField] float lacunarity = 1;
    [SerializeField] float exponent = 1;
    public Vector2 offset = new Vector2();

    [SerializeField] Gradient gradient = new Gradient();
    [SerializeField] AnimationCurve heightCurve = new AnimationCurve();

    [SerializeField] int erosionIterations = 1000;

    public PlaceableObject[] objects;

    private Mesh GenerateMeshOnly()
    {
        //noise maps
        float[] tempHeightMap = Noise.Generate(size + 1, seed, scale, octaves, persistance, lacunarity, offset);

        if(shouldErode) GetComponent<Erosion>().Erode(tempHeightMap, size, erosionIterations);

        float[,] heightMap = Unflatten(tempHeightMap);
        float[,] moistureMap = Unflatten(Noise.Generate(size + 1, seed + 3, biomeScale, 2, persistance, lacunarity, offset));

        Color[] colors = new Color[(int)Mathf.Pow(size + 1, 2)];
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(size + 1, 2)];

        for (int k = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                vertices[k] = new Vector3(x, Mathf.Pow(heightMap[x, y] * heightMultiplier, exponent), y);
                colors[k] = gradient.Evaluate(moistureMap[x, y]);
                k++;
            }
        }

        //I don't understand what happens here
        void PlaceThings(float[,] blueNoise, GameObject[] things, int density, int bMin, int bMax, float shrinkSize)
        {
            for (int yc = 0; yc < size; yc++)
            {
                for (int xc = 0; xc < size; xc++)
                {
                    double max = 0;

                    int R = density;

                    for (int yn = yc - R; yn <= yc + R; yn++)
                    {
                        for (int xn = xc - R; xn <= xc + R; xn++)
                        {
                            if (0 <= yn && yn < size && 0 <= xn && xn < size)
                            {
                                double e = blueNoise[xn, yn];
                                if (e > max) max = e;
                            }
                        }
                    }
                    if (blueNoise[xc, yc] == max)
                    {
                        int g = Mathf.Min(Mathf.RoundToInt(moistureMap[xc, yc] * 10), 9);
                        
                        if(g >= bMin && g <= bMax)
                        {
                            Transform trans = Instantiate(things[g - bMin], new Vector3(xc, heightMap[xc, yc] * heightMultiplier + 1, yc), 
                                things[0].transform.rotation).transform;
                            trans.SetParent(transform, false);
                        }
                    }
                }
            }
        }

        foreach(Transform item in transform)
        {
            DestroyImmediate(item.gameObject, true);
        }

        if (shouldPlace)
        {
            foreach(PlaceableObject item in objects)
            {
                float[,] noise = Unflatten(Noise.Generate(size + 1, seed + 808 + item.seed, scale, 1, persistance, lacunarity, offset));
                PlaceThings(noise, item.objects, item.density, item.min, item.max, item.shrinkSize);
            }
        }

        int[] triangles = new int[size * size * 6];

        //v stands for vertex, and t stands for triangular polygon
        for (int v = 0, t = 0, y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                triangles[t] = v;
                triangles[t + 1] = v + size + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + size + 1;
                triangles[t + 5] = v + size + 2;

                v++;
                t += 6;
            }
            v++;
        }

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            colors = colors
        };

        return mesh;
    }

    public void Generate(bool withCollider = true)
    {
        //TODO add LOD especially for mesh, to reduce lag with chunks
        Mesh mesh = GenerateMeshOnly();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

        if(withCollider) GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void DrawMapInEditor()
    {
        GetComponent<MeshFilter>().sharedMesh = GenerateMeshOnly();
        GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
    }

    static float[,] Unflatten(float[] input)
    {
        int size = (int)Mathf.Sqrt(input.Length);
        float[,] result = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                result[x, y] = input[y * size + x];
            }
        }

        return result;
    }
}

[Serializable]
public class PlaceableObject
{
    public GameObject[] objects;
    public int min;
    public int max;
    public int density;
    public int seed;
    public float shrinkSize;
}

#if UNITY_EDITOR
    [CustomEditor(typeof(MeshGenerator))]
    public class MapGeneratorEditor : Editor
    {
        bool autoupdate = false;
        public override void OnInspectorGUI()
        {
            MeshGenerator meshGen = (MeshGenerator)target;

            autoupdate = EditorGUILayout.Toggle("Autoupdate", autoupdate);

            if (DrawDefaultInspector() && autoupdate)
            {
                meshGen.DrawMapInEditor();
            }

            if (GUILayout.Button("Generate"))
            {
                meshGen.DrawMapInEditor();
            }
        }
    }
#endif