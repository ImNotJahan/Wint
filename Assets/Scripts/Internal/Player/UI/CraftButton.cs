using System.Collections.Generic;
using UnityEngine;

public class CraftButton : MonoBehaviour
{
    public List<Item> itemsNeeded = new List<Item>();
    public Item result;

    public void Craft()
    {
        foreach(Item item in itemsNeeded)
        {
            PlayerMovementScript.instance.GetComponent<CharacterCombat>().myStats.inventory.Remove(item);
        }

        PlayerMovementScript.instance.GetComponent<CharacterCombat>().myStats.inventory.Add(result);
    }
}
