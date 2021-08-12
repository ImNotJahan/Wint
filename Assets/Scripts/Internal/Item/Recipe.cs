using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Recipe : ScriptableObject
{
    public Item result;
    public List<Item> ingredients;
}
