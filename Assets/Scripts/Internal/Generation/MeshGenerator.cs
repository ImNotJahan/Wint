using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public GameObject tree;

    [SerializeField] float heightMultiplier = 1;
    [SerializeField] int size = 100;
    [SerializeField] int seed = 42;
    [SerializeField] float scale = 1;
    [SerializeField] float biomeScale = 1;
    [SerializeField] int octaves = 1;
    [SerializeField] float persistance = 1;
    [SerializeField] float lacunarity = 1;
    [SerializeField] Vector2 offset = new Vector2();

    [SerializeField] Gradient[] gradients = new Gradient[3];

    Mesh map;

    private void Start()
    {
        map = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = map;

        Generate();
    }

    private Mesh GenerateMeshOnly()
    {
        //noise maps
        float[,] heightMap = Noise.Generate(size + 1, seed, scale, octaves, persistance, lacunarity, offset);

        float[,] moistureMap = Noise.Generate(size + 1, seed + 3, biomeScale, 2, persistance, lacunarity, offset);

        Color[] colors = new Color[(int)Mathf.Pow(size + 1, 2)];
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(size + 1, 2)];

        for (int k = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                int g = Mathf.Min(Mathf.RoundToInt(moistureMap[x, y] * 10), 9);

                vertices[k] = new Vector3(x, heightMap[x, y] * heightMultiplier, y);
                colors[k] = gradients[g].Evaluate(vertices[k].y / heightMultiplier);
                k++;
            }
        }

        float[,] blueNoise = Noise.Generate(size + 6, seed, scale, 1, persistance, lacunarity, offset);

        //I don't understand what happens here
        for (int yc = 0; yc < size; yc++)
        {
            for (int xc = 0; xc < size; xc++)
            {
                double max = 0;

                int R = 4;

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
                    Instantiate(tree, new Vector3(xc, heightMap[xc, yc] * heightMultiplier + 1, yc), tree.transform.rotation);
                }
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

    private void Generate()
    {
        Mesh mesh = GenerateMeshOnly();

        map.Clear();
        map = mesh;
        map.RecalculateNormals();
    }

    public void DrawMapInEditor()
    {
        GetComponent<MeshFilter>().sharedMesh = GenerateMeshOnly();
        GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
    }
}

[CustomEditor(typeof(MeshGenerator))]
public class MapGeneratorEditor : Editor
{
    bool autoupdate = false;
    public override void OnInspectorGUI()
    {
        MeshGenerator meshGen = (MeshGenerator)target;

        EditorGUILayout.Toggle(autoupdate);

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