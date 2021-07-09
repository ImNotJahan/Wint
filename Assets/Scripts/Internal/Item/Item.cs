using UnityEngine;

public class Item : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && Input.GetKeyUp(IniFiles.Keybinds.interact))
        {
            PickUp(collider);
        }
    }

    public void PickUp(Collider collider)
    {
        collider.GetComponent<CharacterCombat>().myStats.inventory.Add(this);
    }
}
