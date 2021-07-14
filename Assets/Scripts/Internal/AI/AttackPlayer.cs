using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AttackPlayer : MonoBehaviour
{
    Transform target;
    CharacterController controller;
    Animator anim;

    public CharacterCombat combat;
    public int maxDistance = 20;
    public int attackDistance = 2;
    public int speed = 4;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if(target != null)
        {
            anim.SetBool("canSeePlayer", true);
            anim.SetFloat("playerDst", Vector3.Distance(transform.position, target.position));

            if (anim.GetFloat("playerDst") < maxDistance)
            {
                Vector3 movement = transform.forward * speed * Time.deltaTime;
                movement.y = 0;
                controller.Move(movement);
            }

            if (Vector3.Distance(transform.position, target.position) < 1.5)
            {                
                if(Vector3.Distance(transform.position, target.position) > attackDistance - .2)
                {
                    Vector3 movement = transform.forward * speed * Time.deltaTime;
                    movement.y = 0;
                    controller.Move(movement);
                }
            }
            else
            {
                transform.LookAt(target);
            }
        }
        else
        {
            anim.SetBool("canSeePlayer", false);
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject ally in allies)
            {
                anim.SetBool("lookingForPlayer", true);
                if(Vector3.Distance(transform.position, ally.transform.position) < maxDistance)
                {
                    target = ally.transform;
                    break;
                }
            }

            if(target == null)
            {
                anim.SetBool("lookingForPlayer", false);
            }
        }
    }
}
