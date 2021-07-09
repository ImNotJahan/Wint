using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool onGround = true;
    public string weaponId;
    public Animation anim;

    public int baseDamage = 10;
    public int variation = 3;
    public float attackSpeed = 1f;

    public WeaponType type = WeaponType.Slicing;
    public bool magic = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CharacterCombat>().myStats.equipedWeapon = weaponId;
            transform.parent = other.GetComponent<CharacterCombat>().equipped;
            transform.localPosition = Vector3.zero;
            transform.rotation.Set(90, 0, 0, 0);
        }
    }
}

public enum WeaponType
{
    Slicing,
    Stabbing,
    Short,
    Blunt
}
