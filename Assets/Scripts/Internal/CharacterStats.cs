using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    private static string directorypath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                @"\Jahan Rashidi\Wint\Saves\";

    public static CharacterStats currentPlayerInstance;

    public ParamEvent onTakeDamage = new ParamEvent();

    public PlayerMovementScript movementScript;
    public CharacterCombat characterCombat;

    public string entityId;

    public int maxHealth = 100;
    public int health = 100;
    public int damage = 10;
    public int defense = 0;
    public float attackSpeed = 1f;
    public float damageSpeed = 0.2f;

    public string equipedWeapon = "";
    public Weapon weapon;

    public List<Item> inventory = new List<Item>();
    public List<Recipe> knownRecipes = new List<Recipe>();
    public List<Quest> log = new List<Quest>();
    public List<string> achievements = new List<string>();

    public bool dropItemsOnDeath = false;
    public bool isMonster = true;

    public int AttackPower()
    {
        if (weapon != null)
        {
            return damage + weapon.baseDamage + UnityEngine.Random.Range(-weapon.variation, weapon.variation);
        }
        else
        {
            return damage + UnityEngine.Random.Range(-3, 3);
        }
    }

    public void TakeDamage(int damage)
    {
        damage += UnityEngine.Random.Range(-3, 3);
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

    public void Save()
    {
        if (!Directory.Exists(directorypath))
        {
            Directory.CreateDirectory(directorypath);
        }

        string data = JsonUtility.ToJson(this);
        File.WriteAllText(directorypath + "Save0.json", data);
    }

    public void Load()
    {
        string data = File.ReadAllText(directorypath + "Save0.json");
        JsonUtility.FromJsonOverwrite(data, this);
    }
}