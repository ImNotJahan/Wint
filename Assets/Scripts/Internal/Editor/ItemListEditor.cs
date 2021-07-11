using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemList))]
public class ItemListEditor : Editor
{
    Item item = new Item();
    string id;
    string itemIdToRemove;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        foreach(string itemId in ItemList.items.Keys)
        {
            Item item = ItemList.items[itemId];
            GUILayout.Label(string.Format("{1}x {0} - {2}", item.item_id, item.count, itemId));
        }

        EditorGUILayout.Separator();

        item = EditorGUILayout.ObjectField("Item", item, typeof(Item), true) as Item;
        id = EditorGUILayout.TextField("Id", id);
        if (GUILayout.Button("Add item"))
        {
            ItemList.items.Add(id, item);
        }

        EditorGUILayout.Space();

        itemIdToRemove = EditorGUILayout.TextField("Id", itemIdToRemove);
        if (GUILayout.Button("Remove item"))
        {
            ItemList.items.Remove(itemIdToRemove);
        }
    }
}
