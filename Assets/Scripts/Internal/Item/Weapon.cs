using UnityEngine;

public class Weapon : Item
{
    public bool onGround = true;
    public string weaponId;
    public Animation anim;

    public int baseDamage = 10;
    public int variation = 3;
    public float attackSpeed = 1f;

    public WeaponType type = WeaponType.Slicing;
    public bool magic = false;

    public override void PickUp(Collider collider)
    {
        base.PickUp(collider);
        collider.GetComponent<CharacterCombat>().myStats.equipedWeapon = weaponId;
        transform.parent = collider.GetComponent<CharacterCombat>().equipped;
        transform.rotation.Set(90, 0, 0, 0);
        transform.localPosition = Vector3.zero;
    }
}

public enum WeaponType
{
    Slicing,
    Stabbing,
    Short,
    Blunt
}
