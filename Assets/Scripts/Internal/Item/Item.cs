using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    public GameObject gameItem;
    public string item_id;
    public string description;

    public int count;
    public float rarity;
}
