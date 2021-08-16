using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Recipe : ScriptableObject
{
    public Item result;
    public List<Item> ingredients;
    public Utility utility; //what the recipe can be used in
}

public enum Utility
{
    Inventory,
    Furance,
    Anvil,
    Table,
    Crucible
}