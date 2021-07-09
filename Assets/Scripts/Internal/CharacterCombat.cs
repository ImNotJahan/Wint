using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public CharacterStats myStats = new CharacterStats();
    public Transform equipped;
    private float attackCooldown = 0;

    private void Start()
    {
        myStats.characterCombat = this;
    }

    public void Attack(CharacterStats targetStats)
    {
        if(attackCooldown <= 0)
        {
            if(targetStats != null) targetStats.TakeDamage(myStats.AttackPower());
            if (myStats.weapon == null) attackCooldown = myStats.attackSpeed;
            else attackCooldown = myStats.weapon.attackSpeed;

            if (!string.IsNullOrEmpty(myStats.equipedWeapon))
            {
                equipped.GetChild(0).GetComponent<Weapon>().anim.Play();
            }
        }
    }

    public void die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
}