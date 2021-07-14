﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCombat : MonoBehaviour
{
    public CharacterStats myStats = new CharacterStats();
    public Transform equipped;
    private float attackCooldown = 0;
    public GameObject cooldownBar;
    private Image cooldownBarImage;

    private void Start()
    {
        myStats.characterCombat = this;
        if (cooldownBar != null) cooldownBarImage = 
                cooldownBar.transform.GetChild(0).GetComponent<Image>();
    }

    public void Attack()
    {
        if(attackCooldown <= 0)
        {

            attackCooldown = myStats.attackSpeed;

            if (!string.IsNullOrEmpty(myStats.equipedWeapon))
            {
                Weapon weapon = equipped.GetChild(0).GetComponent<Weapon>();
                weapon.Attack(myStats.AttackPower());
                weapon.anim.Stop();
                weapon.anim.Play();

                attackCooldown = weapon.attackSpeed;
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

        AchievementHandler.entityDeath.Invoke(new string[] { myStats.entityId });

        Destroy(gameObject);
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (cooldownBar != null)
        {
            if (attackCooldown > 0)
            {
                cooldownBar.SetActive(true);
                cooldownBarImage.fillAmount = (1 - attackCooldown) / ((myStats.weapon == null) ? myStats.attackSpeed :
                    myStats.weapon.attackSpeed);
            }
            else
            {
                cooldownBar.SetActive(false);
            }
        }
    }
}