using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PickableItem
{
    [SerializeField] private int healing = 0;
    public override void PickUp(Collider collider)
    {
        CharacterStats stats = collider.GetComponent<CharacterCombat>().myStats;
        stats.inventory.Add(item);

        held = true;
        Destroy(gameObject);
    }
}
