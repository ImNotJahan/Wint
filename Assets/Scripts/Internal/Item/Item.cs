using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject gameItem;
    public string item_id;
    public string description;

    public int count = 1;

    bool held = false;

    public void OnTriggerStay(Collider collider)
    {
        if (!held)
        {
            if (collider.tag == "Player" && Input.GetKeyUp(IniFiles.Keybinds.interact))
            {
                PickUp(collider);
            }
        }
    }

    public virtual void PickUp(Collider collider)
    {
        collider.GetComponent<CharacterCombat>().myStats.inventory.Add(this);
        held = true;
    }
}
