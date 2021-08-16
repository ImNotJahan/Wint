using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftItem : MonoBehaviour
{
    public Text craftTitle;
    public Text craftIngredients;
    public CraftButton button;

    public Item title;
    public List<Item> ingredients = new List<Item>();
    public int count = 1;

    public void DisplayItem()
    {
        if (count != 1) craftTitle.text = string.Format("{0} {1}x", title.item_id, count.ToString());
        else craftTitle.text = title.item_id;

        string ingredientsList = "";

        foreach(Item ingredient in ingredients)
        {
            ingredientsList += ingredient.item_id + "\n";
        }

        craftIngredients.text = ingredientsList;

        bool hasIngredients = true;

        List<Item> inventoryCopy = PlayerMovementScript.instance.GetComponent<CharacterCombat>().myStats.inventory;
        foreach (Item item in ingredients)
        {
            if (inventoryCopy.Contains(item)) inventoryCopy.Remove(item);
            else hasIngredients = false;
        }

        button.GetComponent<Button>().interactable = hasIngredients;
        button.itemsNeeded = ingredients;
        button.result = title;
        button.onCraft.AddListener(DisplayItem); //to check if the player still has enough ingredients to craft
    }
}
