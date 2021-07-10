using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject gameItem;
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
