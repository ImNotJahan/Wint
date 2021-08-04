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
        //noise maps
        float[,] noiseMap = Noise.Generate(size + 1, seed, scale, octaves, persistance, lacunarity, offset);
        float[,] mountainMap = Noise.Generate(size + 1, seed + 1, scale * 1000, octaves, persistance, lacunarity, offset);
        float[,] lakeMap = Noise.Generate(size + 1, seed + 2, scale, octaves, persistance, lacunarity, offset);

        Color[] colors = new Color[(int)Mathf.Pow(size + 1, 2)];
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(size + 1, 2)];

        for (int k = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                float trueY = noiseMap[x, y] * heightMultiplier; //this is for the vertex's y

                if (mountainMap[x, y] > 0.2)
                {
                    trueY += mountainMap[x, y] * 10 * heightMultiplier;
                }

                vertices[k] = new Vector3(x, trueY, y);
                colors[k] = gradients[0].Evaluate(vertices[k].y / heightMultiplier);
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

        

        Vector3[] biomeVertices = vertices;

        int amountOfIterations = 1;
        int g = Random.Range(0, gradients.Length - 1);

        Vector2 startingPoint = new Vector2(10, 10);

        /*for (int i = 0; i < amountOfIterations; i++)
        {
            for (int k = 0, y = 0; y < size + 1; y++)
            {
                for (int x = 0; x < size + 1; x++)
                {
                    if (Mathf.Pow(x - startingPoint.x, 2) + Mathf.Pow(y - startingPoint.y, 2) <= Mathf.Pow(2, 2)
                        && biomeVertices[k].y != 2)
                    {
                        //TODO choose gradient using a seed or something to have repeatable results
                        colors[k] = gradients[g].Evaluate(biomeVertices[k].y / heightMultiplier);
                        //biomeVertices[k].y = 2;
                        k++;
                    }
                }
            }
        }*/

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