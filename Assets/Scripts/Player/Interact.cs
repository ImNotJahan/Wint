using UnityEngine;

public class Interact : MonoBehaviour
{
    public UIHandler uihandler;
    public void ProcessInteract(Transform hit)
    {
        if (hit.GetComponent<NPCBase>() != null)
        {
            NPCInteract(hit.GetComponent<NPCBase>());
        }
    }

    public void NPCInteract(NPCBase npc)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;

        uihandler.displayMessage(npc.dialogs[0], npc.name, 0);
    }
}
