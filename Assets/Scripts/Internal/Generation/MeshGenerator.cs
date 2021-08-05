using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public GameObject[] trees;
    public GameObject rock;

    public bool shouldPlace = false;

    [SerializeField] float heightMultiplier = 1;
    [SerializeField] int size = 100;
    [SerializeField] int seed = 42;
    [SerializeField] float scale = 1;
    [SerializeField] float biomeScale = 1;
    [SerializeField] int octaves = 1;
    [SerializeField] float persistance = 1;
    [SerializeField] float lacunarity = 1;
    [SerializeField] Vector2 offset = new Vector2();

    [SerializeField] Gradient gradient = new Gradient();

    Transform parent;

    Mesh map;

    private void Start()
    {
        map = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = map;

        Generate();
    }

    private Mesh GenerateMeshOnly()
    {
        if(parent == null)
        {
            parent = new GameObject().transform;
        }

        //noise maps
        float[,] heightMap = Noise.Generate(size + 1, seed, scale, octaves, persistance, lacunarity, offset);

        float[,] moistureMap = Noise.Generate(size + 1, seed + 3, biomeScale, 2, persistance, lacunarity, offset);

        Color[] colors = new Color[(int)Mathf.Pow(size + 1, 2)];
        Vector3[] vertices = new Vector3[(int)Mathf.Pow(size + 1, 2)];

        for (int k = 0, y = 0; y <= size; y++)
        {
            for (int x = 0; x <= size; x++)
            {
                vertices[k] = new Vector3(x, heightMap[x, y] * heightMultiplier, y);
                colors[k] = gradient.Evaluate(moistureMap[x, y]);
                k++;
            }
        }

        float[,] treeNoise = Noise.Generate(size + 1, seed + 6, scale, 1, persistance, lacunarity, offset);
        float[,] rockNoise = Noise.Generate(size + 1, seed + 7, scale, 1, persistance, lacunarity, offset);

        //I don't understand what happens here
        void PlaceThings(float[,] blueNoise, GameObject[] things, int density, int bMin, int bMax)
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
                            Instantiate(things[g - bMin], new Vector3(xc, heightMap[xc, yc] * heightMultiplier + 1, yc), things[0].transform.rotation, parent);
                        }
                    }
                }
            }
        }

        foreach(Transform item in parent)
        {
            DestroyImmediate(item.gameObject, true);
        }

        if (shouldPlace)
        {
            PlaceThings(treeNoise, trees, 4, 3, 7);
            //PlaceThings(rockNoise, rocks, 4);
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

#if UNITY_EDITOR
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
#endif