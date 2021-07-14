using UnityEngine;

public class Weapon : PickableItem
{
    public bool onGround = true;
    public string weaponId;
    public Animation anim;

    public GameObject bloodEffect;

    public int baseDamage = 10;
    public int variation = 3;
    public float attackSpeed = 1f;

    public bool magic = false;

    private int damage = 0;
    private float time = 0;

    public override void PickUp(Collider collider)
    {
        base.PickUp(collider);
        collider.GetComponent<CharacterCombat>().myStats.equipedWeapon = weaponId;
        transform.parent = collider.GetComponent<CharacterCombat>().equipped;
        transform.rotation.Set(90, 0, 0, 0);
        transform.localPosition = Vector3.zero;
    }

    public void Attack(int damage)
    {
        this.damage = damage;
        time = 0;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (time <= attackSpeed)
        {
            CharacterStats targetStats = collision.transform.GetComponent<CharacterCombat>().myStats;
            if (targetStats != null)
            {
                targetStats.TakeDamage(damage);

                //Vector3 pos = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                //Instantiate(bloodEffect, pos, Quaternion.identity, collision.transform);
            }
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
    }
}