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
        if (myStats.dropItemsOnDeath)
        {
            Vector3 pos = transform.position + new Vector3(0, 1, 0);
            if (equipped != null)
            {
                Weapon weapon = equipped.GetChild(0).GetComponent<Weapon>();
                Instantiate(weapon).transform.position = pos;
            }

            foreach (Item item in myStats.inventory)
            {
                if (Random.Range(0f, 1f) <= item.rarity)
                {
                    Instantiate(item.gameItem).transform.position = pos;
                }
            }
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
}