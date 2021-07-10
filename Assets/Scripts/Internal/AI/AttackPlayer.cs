using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    Transform target;
    CharacterController controller;

    public CharacterCombat combat;
    public int maxDistance = 20;
    public int attackDistance = 2;
    public int speed = 4;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target);

            if (Vector3.Distance(transform.position, target.position) < maxDistance)
            {
                if (Vector3.Distance(transform.position, target.position) < attackDistance)
                {
                    combat.Attack(target.GetComponent<CharacterCombat>().myStats);
                }
                else
                {
                    Vector3 movement = transform.forward * speed * Time.deltaTime;
                    movement.y = 0;
                    controller.Move(movement);
                }
            }
        }
        else
        {
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject ally in allies)
            {
                if(Vector3.Distance(transform.position, ally.transform.position) < maxDistance)
                {
                    target = ally.transform;
                    break;
                }
            }
        }
    }
}
