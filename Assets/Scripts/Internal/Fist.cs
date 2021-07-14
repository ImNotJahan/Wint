﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public int damage = 10;
    public GameObject bloodEffect;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.transform.root);
        if (collision.transform.root == transform.root)
            return;

        CharacterStats targetStats = collision.transform.GetComponent<CharacterCombat>().myStats;
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage);

            //Vector3 pos = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            //Instantiate(bloodEffect, pos, Quaternion.identity, collision.transform);
            //Instantiate(bloodEffect, pos, Quaternion.identity, collision.transform);
        }
    }
}
