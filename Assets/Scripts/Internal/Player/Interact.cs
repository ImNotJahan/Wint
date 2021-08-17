using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UIHandler uihandler;
    public CharacterCombat characterCombat;
    public static UnityEvent furnaceInteractedWith;
    public static UnityEvent anvilInteractedWith;
    public static UnityEvent crucibleInteractedWith;

    private void Awake()
    {
        furnaceInteractedWith = new UnityEvent();
        crucibleInteractedWith = new UnityEvent();
        anvilInteractedWith = new UnityEvent();
    }

    private void Start()
    {
        characterCombat = transform.parent.GetComponent<CharacterCombat>();
        characterCombat.myStats.isMonster = false;
    }

    public void ProcessInteract(Transform hit)
    {
        if (!hit.gameObject.isStatic)
        {
            if (hit.GetComponent<NPCBase>() != null)
            {
                transform.LookAt(hit.GetComponent<NPCBase>().head);
                NPCInteract(hit.GetComponent<NPCBase>());
            }
            else if (hit.GetComponent<PickableItem>())
            {
                hit.GetComponent<PickableItem>().PickUp(transform.parent.GetComponent<Collider>());
            } else if(hit.tag == "Furnace")
            {
                furnaceInteractedWith.Invoke();
            } else if (hit.tag == "Anvil")
            {
                anvilInteractedWith.Invoke();
            } else if (hit.tag == "Crucible")
            {
                crucibleInteractedWith.Invoke();
            }
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
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3))
            {
                transform.GetComponent<Interact>().ProcessInteract(hit.transform);
            }
        }

        if (Input.GetMouseButtonDown(0) && uihandler.menuIndex == 0)
        {
            characterCombat.Attack();
        }
    }
}
