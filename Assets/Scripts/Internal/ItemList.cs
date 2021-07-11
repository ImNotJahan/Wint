using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemList : ScriptableObject
{
    static public Dictionary<string, Item> items;

    private void OnEnable()
    {
        if (items == null)
        {
            items = new Dictionary<string, Item>();
        }
    }
}
