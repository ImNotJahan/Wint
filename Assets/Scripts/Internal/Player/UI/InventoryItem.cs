﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Text invTitle;
    public Text invDescription;

    public GameObject itemPrefab;
    public Item itemSelf;

    public string title = "";
    public string description = "";
    public int count = 1;

    public UnityEvent onItemDrop = new UnityEvent();

    public void DisplayItem()
    {
        if (count != 1) invTitle.text = string.Format("{0} {1}x", title, count.ToString());
        else invTitle.text = title;
        invDescription.text = description;
    }

    public void Drop()
    {
        CharacterStats.currentPlayerInstance.characterCombat.myStats.inventory.Remove(itemSelf);

        PickableItem possiblyHeldObject;
        if (itemSelf.gameItem.TryGetComponent(out possiblyHeldObject) && possiblyHeldObject != null && possiblyHeldObject.held)
        {
            Destroy(possiblyHeldObject.gameObject);
        }

        Instantiate(itemPrefab);

        onItemDrop.Invoke();
    }

    public void Equip()
    {
        Transform equipSlot = PlayerMovementScript.instance.transform.GetChild(3);

        Destroy(equipSlot.GetChild(0).gameObject);

        Transform weapon = Instantiate(itemPrefab, equipSlot).transform;
        weapon.position = Vector3.zero;
        transform.rotation.Set(90, 0, 0, 0);
    }
}
