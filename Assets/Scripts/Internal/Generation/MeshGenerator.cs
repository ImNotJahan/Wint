using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] float heightMultiplier = 1;
    [SerializeField] public static int size = 100;
    [SerializeField] public static int seed = 42;
    [SerializeField] float scale = 1;
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
        float[,] noiseMap = Noise.Generate(size + 1, seed, scale, octaves, persistance, lacunarity, offset);
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(size + 1, 2)];

        for (int k = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                vertices[k] = new Vector3(x, noiseMap[x, y] * heightMultiplier, y);

                k++;
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

        Color[] colors = new Color[vertices.Length];
        int[,] biomeMap = Biomes.CreateBiomeMap();

        for (int k = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                colors[k] = gradients[biomeMap[x, y]].Evaluate(vertices[k].y / heightMultiplier);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

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

    public override void OnInspectorGUI()
    {
        MeshGenerator meshGen = (MeshGenerator)target;

        if (DrawDefaultInspector())
        {
            meshGen.DrawMapInEditor();
        }

        MeshGenerator.size = EditorGUILayout.IntField(MeshGenerator.size);
        MeshGenerator.seed = EditorGUILayout.IntField(MeshGenerator.seed);

        if (GUILayout.Button("Generate"))
        {
            meshGen.DrawMapInEditor();
        }
    }
}