using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    public GameObject gameItem;
    public string itemName;
    public string description;

    public int count;
    public float rarity;
}