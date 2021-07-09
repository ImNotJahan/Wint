using UnityEngine;

public class Interact : MonoBehaviour
{
    public UIHandler uihandler;
    public CharacterCombat characterCombat;

    private void Start()
    {
        characterCombat = transform.parent.GetComponent<CharacterCombat>();
        characterCombat.myStats.isMonster = false;
    }

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

        uihandler.displayMessage(npc.dialogs[0], npc.name, npc.responses[0], npc.HandleClick, npc, 1);
    }

    private void Update()
    {
        if (Input.GetKeyUp(IniFiles.Keybinds.interact))
        {
            int layerMask = 1 << 8;

            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                transform.GetComponent<Interact>().ProcessInteract(hit.transform);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            bool didHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3);
            if (didHit && hit.transform.GetComponent<CharacterCombat>() != null)
            {
                characterCombat.Attack(hit.transform.GetComponent<CharacterCombat>().myStats);
            }
            else
            {
                characterCombat.Attack(null);
            }
        }
    }
}
