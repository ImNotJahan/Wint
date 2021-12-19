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

    /** Calculates how much damage an attack from this entity would deal **/
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
        damage += UnityEngine.Random.Range(-3, 3); // Add some variation
        health -= Mathf.Max(damage - defense, 1); // Makes sure you don't get healed from weak attacks

        // Checking for death
        if (health < 1)
        {
            // Monsters and players handle death differently
            if (isMonster) characterCombat.die();
            else movementScript.die();
        }

        onTakeDamage.Invoke(new string[] { });
    }

    public void Heal(int hp)
    {
        hp += UnityEngine.Random.Range(-3, 3); // Add a little variation to the healing
        health = Mathf.Min(health + hp, maxHealth); // Makes sure that the players health doesn't go over 
    }

    /** Returns the entityId **/
    public override string ToString()
    {
        return entityId;
    }

    /** Saves all stats into a json **/
    public void Save()
    {
        if (!Directory.Exists(directorypath))
        {
            Directory.CreateDirectory(directorypath);
        }

        string data = JsonUtility.ToJson(this);
        File.WriteAllText(directorypath + "Save0.json", data);
    }

    /** Loads all stats from a json **/
    public void Load()
    {
        string data = File.ReadAllText(directorypath + "Save0.json");
        JsonUtility.FromJsonOverwrite(data, this);
    }
}