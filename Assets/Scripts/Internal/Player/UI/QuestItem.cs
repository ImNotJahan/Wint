using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    public Text questTitle;
    public Text questDescription;

    public string title = "";
    public string description = "";

    public void DisplayItem()
    {
        questTitle.text = title;
        questDescription.text = description;
    }
}
