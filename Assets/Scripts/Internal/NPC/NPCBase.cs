using System;
using UnityEngine;
using static MultidimensionalArray;

public class NPCBase : MonoBehaviour
{
    public string npcName;
    public UIHandler uihandler;

    public ParamEvent onDialog = new ParamEvent();

    public string[] dialogs = new string[] { };
    public Arr[] responses = new Arr[] { };

    [NonSerialized]
    public int responseId = 0;
    [NonSerialized]
    public static int responseId2 = 0;

    /*How does this work?
     I'm not completely sure myself but basically what happens is it displays the message and responses
     based off both of the response ids added together. The first response id is 1+3*k basically,
     because there are max 3 responses to a dialog, and the 1 part I'm not sure about. ResonseId2
     is just whichever response the user clicks on eg the first one would be 0, the second 1, and the
     third 2.*/
    public void HandleClick()
    {
        string dialog = dialogs[responseId + responseId2];
        uihandler.displayMessage(dialog, npcName, 
            responses[responseId + responseId2], HandleClick, this, responseId + 3);
        onDialog.Invoke(new string[] { dialog });
    }
}