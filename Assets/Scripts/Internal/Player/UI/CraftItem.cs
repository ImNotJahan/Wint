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
        craftIngredients.text = ingredients.ToString();

        button.itemsNeeded = ingredients;
        button.result = title;
    }
}
