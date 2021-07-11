using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Quest quest = (Quest)target;
        if (GUILayout.Button("Complete quest"))
        {
            quest.Complete();
        }
    }
}
