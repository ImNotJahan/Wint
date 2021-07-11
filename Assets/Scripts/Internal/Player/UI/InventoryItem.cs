using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Text invTitle;
    public Text invDescription;

    public string title = "";
    public string description = "";
    public int count = 1;

    public void DisplayItem()
    {
        if (count != 1) invTitle.text = string.Format("{0} {1}x", title, count.ToString());
        else invTitle.text = title;
        invDescription.text = description;
    }
}
