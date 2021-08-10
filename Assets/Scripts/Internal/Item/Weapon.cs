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

    public Element[] strengths;
    public ToolType toolType;

    public override void PickUp(Collider collider)
    {
        collider.GetComponent<CharacterCombat>().myStats.inventory.Add(item);
        held = true;

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
            CharacterCombat combat = collision.GetComponent<CharacterCombat>();
            if (combat != null)
            {
                combat.myStats.TakeDamage(Mathf.RoundToInt(damage + baseDamage * Evaluate(toolType, 0)));

                //Vector3 pos = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                //Instantiate(bloodEffect, pos, Quaternion.identity, collision.transform);
            } else if(collision.GetComponent<BreakableItem>() != null)
            {
                collision.GetComponent<BreakableItem>().Attack(Mathf.RoundToInt(damage + baseDamage * Evaluate(toolType, 1)));
            }
        }
    }

    private float Evaluate(ToolType toolType, int attacking)
    {
        switch (attacking)
        {
            case 0:
                switch (toolType)
                {
                    case ToolType.Sword:
                        return 1f;

                    case ToolType.Axe:
                        return 0.9f;

                    case ToolType.Pickaxe:
                        return 0.7f;

                    case ToolType.Shovel:
                        return 0.5f;
                }
                break;

            case 1:
                switch (toolType)
                {
                    case ToolType.Sword:
                        return 0.3f;

                    case ToolType.Axe:
                        return 1f;

                    case ToolType.Pickaxe:
                        return 0.7f;

                    case ToolType.Shovel:
                        return 0.5f;
                }
                break;
        }

        return 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }
}

public enum ToolType
{
    Sword,
    Axe,
    Pickaxe,
    Shovel
}