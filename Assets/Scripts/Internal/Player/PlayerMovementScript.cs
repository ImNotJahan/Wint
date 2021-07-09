﻿using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private CharacterController controller;

    [Range(1f, 20f)]
    public float speed = 12f;
    float currentSpeed;

    [Range(-30f, -5f)]
    public float gravity = -19.62f;

    [Range(1f, 10f)]
    public float jumpHeight = 3f;

    public Transform groundCheck;

    [Range(0.1f, 1f)]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public bool disabled = false;
    public CharacterStats characterStats = new CharacterStats();

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        characterStats.movementScript = this;
    }

    void Update()
    {
        if (!disabled)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            currentSpeed = speed;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetKey(IniFiles.Keybinds.sprint))
            {
                currentSpeed = speed * 1.58f;
            }

            if (Input.GetKey(IniFiles.Keybinds.crouch))
            {
                currentSpeed = speed * 0.58f;
                controller.height = .7f;
            }
            else { controller.height = 2; }

            //TODO add smoothing function to movement
            float x = 0;
            float z = 0;

            if (Input.GetKey(IniFiles.Keybinds.forward))
            {
                z = 1;
            }
            else if (Input.GetKey(IniFiles.Keybinds.backward))
            {
                z = -1;
            }

            if (Input.GetKey(IniFiles.Keybinds.right))
            {
                x = 1;
            }
            else if (Input.GetKey(IniFiles.Keybinds.left))
            {
                x = -1;
            }

            Vector2 smoothedInput = SmoothInput(x, z);
            x = smoothedInput.x;
            z = smoothedInput.y;

            if (Input.GetKeyDown(IniFiles.Keybinds.jump) && isGrounded && Time.timeScale == 1f)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * currentSpeed * Time.deltaTime);
        }
    }

    public static Vector3[] spawnpoints;
    public void die()
    {
        characterStats = new CharacterStats();
        characterStats.movementScript = this;

        transform.position = spawnpoints[Random.Range(0, spawnpoints.Length - 1)];
    }

    private Vector2 SmoothInput(float targetH, float targetV)
    {
        float h = 0;
        float v = 0;
        float deadZone = 0.001f;

        h = Mathf.MoveTowards(h,
                      targetH, 1000 * Time.deltaTime);

        v = Mathf.MoveTowards(v,
                      targetV, 1000 * Time.deltaTime);

        return new Vector2(
               (Mathf.Abs(h) < deadZone) ? 0f : h,
               (Mathf.Abs(v) < deadZone) ? 0f : v);
    }
}

public class CharacterStats
{
    public PlayerMovementScript movementScript;
    public CharacterCombat characterCombat;

    public int health = 100;
    public int damage = 10;
    public int defense = 0;
    public float attackSpeed = 1f;

    public string equipedWeapon = "";
    public Weapon weapon;

    public bool isMonster = true;

    public int AttackPower()
    {
        if(weapon != null)
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

        if(health < 1)
        {
            if (isMonster) characterCombat.die();
            else movementScript.die();
        }
    }
}