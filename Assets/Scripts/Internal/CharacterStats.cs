﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public ParamEvent onTakeDamage = new ParamEvent();

    public PlayerMovementScript movementScript;
    public CharacterCombat characterCombat;

    public string entityId;

    public int maxHealth = 100;
    public int health = 100;
    public int damage = 10;
    public int defense = 0;
    public float attackSpeed = 1f;

    public string equipedWeapon = "";
    public Weapon weapon;

    public List<Item> inventory = new List<Item>();

    public bool dropItemsOnDeath = false;
    public bool isMonster = true;

    public int AttackPower()
    {
        if (weapon != null)
        {
            return damage + weapon.baseDamage + Random.Range(-weapon.variation, weapon.variation);
        }
        else
        {
            return damage + Random.Range(-3, 3);
        }
    }

    public void TakeDamage(int damage)
    {
        damage += Random.Range(-3, 3);
        health -= Mathf.Max(damage - defense, 1);

        if (health < 1)
        {
            if (isMonster) characterCombat.die();
            else movementScript.die();
        }

        onTakeDamage.Invoke(new string[] { });
    }

    public override string ToString()
    {
        return entityId;
    }
}