using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Item item;

    [HideInInspector] public GameObject gameItem;
    [HideInInspector] public string item_id;
    [HideInInspector] public string description;

    [HideInInspector] public int count = 1;

    private void Start()
    {
        gameItem = item.gameItem;
        item_id = item.item_id;
        description = item.description;
        count = item.count;
    }

    protected bool held = false;

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
        collider.GetComponent<CharacterCombat>().myStats.inventory.Add(item);
        held = true;
        Destroy(gameObject);
    }
}
