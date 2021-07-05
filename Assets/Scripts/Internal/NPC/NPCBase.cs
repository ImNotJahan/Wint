using System;
using UnityEngine;
using static MultidimensionalArray;

public class NPCBase : MonoBehaviour
{
    public string name;
    public UIHandler uihandler;

    public string[] dialogs = new string[] { };
    public int[] dialogIds = new int[] { };
    public Arr[] responses = new Arr[] { };
    public int[] responseIds = new int[] { };

    [NonSerialized]
    public int responseId = 0;
    [NonSerialized]
    public static int responseId2 = 0;

    public void HandleClick()
    {
        switch(responseId + responseId2)
        {
            case 1:
                uihandler.displayMessage(dialogs[1], name, responses[1], HandleClick, this, 4);
                break;

            case 2:
                uihandler.displayMessage(dialogs[2], name, responses[2], HandleClick, this, 7);
                break;

            default:
                uihandler.displayMessage("Response id: " + responseId + " + " + responseId2, name, responses[2], HandleClick, this, 7);
                break;
        }
    }
}