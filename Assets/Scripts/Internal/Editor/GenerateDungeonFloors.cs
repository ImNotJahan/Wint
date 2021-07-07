using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateDungeonFloors : EditorWindow
{
    private Object floor;
    private Transform parent;
    private Object tempParent;

    private bool showingDistanceFields = true;
    private int distanceBetweenFloors = 10;
    private bool variateDistanceBetweenFloors = false;
    private int minDistance = 30;
    private int maxDistance = 140;

    private float floorScaling = 3;
    private int amountOfFloors = 70;
    private bool exponentialScaling = false;

    [MenuItem("Window/Generation/Generate Dungeon Floors")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(GenerateDungeonFloors), 
            false, "Generate Dungeon Floors");
        window.minSize = new Vector2(150, 200);
        window.maxSize = new Vector2(1920, 1080);
    }

    void OnGUI()
    {
        GUILayout.Label("Generation Settings", EditorStyles.boldLabel);

        GUILayout.Label("Floor object");
        floor = EditorGUILayout.ObjectField(floor, typeof(Object), true);

        EditorGUILayout.Separator();

        GUILayout.Label("Floor parent");
        tempParent = EditorGUILayout.ObjectField(tempParent, typeof(GameObject), true);

        EditorGUILayout.Separator();

        showingDistanceFields = EditorGUILayout.Foldout(showingDistanceFields, "Distance fields");

        if (showingDistanceFields)
        {
            EditorGUI.BeginDisabledGroup(variateDistanceBetweenFloors);
            GUILayout.Label("Distance between floors");
            distanceBetweenFloors = EditorGUILayout.IntField(distanceBetweenFloors);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            variateDistanceBetweenFloors = EditorGUILayout.BeginToggleGroup("Variate distance between floors", variateDistanceBetweenFloors);
            minDistance = EditorGUILayout.IntField("Minimum distance", minDistance);
            maxDistance = EditorGUILayout.IntField("Maximum distance", maxDistance);
            EditorGUILayout.EndToggleGroup();
        }

        EditorGUILayout.Separator();

        GUILayout.Label("Amount of floors");
        amountOfFloors = EditorGUILayout.IntField(amountOfFloors);

        EditorGUILayout.Separator();

        GUILayout.Label("Scale factor");
        floorScaling = EditorGUILayout.FloatField(floorScaling);

        EditorGUILayout.Separator();

        exponentialScaling = EditorGUILayout.Toggle("Exponential scaling", exponentialScaling);
        
        if (GUILayout.Button("Generate"))
        {
            parent = ((GameObject)tempParent).transform;

            if(SceneManager.GetActiveScene().name == "Dungeon")
            {
                float lastPow = ((GameObject)floor).transform.localScale.x;

                foreach(Transform child in parent)
                {
                    DestroyImmediate(child.gameObject);
                }

                int distBetweenFloors = distanceBetweenFloors;
                int prevDist = 0;

                for(int k = 0; k < amountOfFloors; k++)
                {
                    EditorUtility.DisplayProgressBar("Generating Dungeon ",
                        "Instantiating " + k + " out of " + amountOfFloors, k / amountOfFloors);

                    if (variateDistanceBetweenFloors)
                    {
                        distBetweenFloors = Random.Range(minDistance, maxDistance);
                    }

                    prevDist = -distBetweenFloors + prevDist;

                    Transform floorInstance = ((GameObject)Instantiate(floor, 
                        new Vector3(0, prevDist, 0), Quaternion.identity, parent)).transform;

                    float size = floorInstance.localScale.x;
                    float height = floorInstance.localScale.y;

                    if (exponentialScaling)
                    {
                        lastPow = Mathf.Pow(lastPow, floorScaling);
                        floorInstance.localScale = new Vector3(lastPow, height, lastPow);
                    }
                    else
                    {
                        floorInstance.localScale = new Vector3(size * k * floorScaling * ((float)distBetweenFloors / distanceBetweenFloors), 
                            height, size * k * floorScaling * ((float)distBetweenFloors / distanceBetweenFloors));
                    }

                    EditorUtility.DisplayProgressBar("Generating Dungeon ", 
                        "Scaling " + k + " out of " + amountOfFloors, k / amountOfFloors);
                }

                EditorUtility.ClearProgressBar();
            }
            else
            {
                Debug.LogWarning("Active scene must be dungeon to generate floors");
            }
        }
    }
}