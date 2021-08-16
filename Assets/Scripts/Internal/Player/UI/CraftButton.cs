using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraftButton : MonoBehaviour
{
    public List<Item> itemsNeeded = new List<Item>();
    public Item result;
    public UnityEvent onCraft = new UnityEvent();

    public void Craft()
    {
        foreach(Item item in itemsNeeded)
        {
            PlayerMovementScript.instance.GetComponent<CharacterCombat>().myStats.inventory.Remove(item);
        }

        PlayerMovementScript.instance.GetComponent<CharacterCombat>().myStats.inventory.Add(result);
        onCraft.Invoke();
    }
}
