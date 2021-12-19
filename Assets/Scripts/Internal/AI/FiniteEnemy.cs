using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class FiniteEnemy : MonoBehaviour
{
    [SerializeField] private CharacterStats stats = new CharacterStats();

    private Animator animator;

    private Ray ray;
    private RaycastHit hit;
    private AnimatorStateInfo info;

    private NavMeshAgent agent;

    private float distance;

    // For dot product of view
    private Vector3 direction;
    private bool inFOV; // FOV = Field of View
    private bool objectBetweenTargetAndSelf = true;

    [SerializeField] private Transform head = null;

    private Transform target;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        stats.onTakeDamage.AddListener(Hit);
    }

    private void Update()
    {
        target = PlayerMovementScript.instance.transform;

        info = animator.GetCurrentAnimatorStateInfo(0);
        
        direction = (target.position - head.position).normalized;
        inFOV = Vector3.Dot(head.forward.normalized, direction) > .7f;

        Debug.DrawRay(head.position, direction * 100, Color.red);
        Debug.DrawRay(head.position, head.forward * 100, Color.red);

        if(Physics.Raycast(head.position, direction * 100, out hit))
        {
            if (hit.collider.gameObject.tag == "Player") objectBetweenTargetAndSelf = false;
            else objectBetweenTargetAndSelf = true;
        }

        distance = Vector3.Distance(transform.position, target.position);
        animator.SetBool("withinReach", distance < 3f && inFOV);

        if (!objectBetweenTargetAndSelf && inFOV)
        {
            animator.SetBool("canSeePlayer", true);
            head.LookAt(target);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
        else animator.SetBool("canSeePlayer", false);

        if (info.IsName("CHASE"))
        {
            if (!inFOV) animator.SetBool("canSeePlayer", false);
            else
            {
                agent.destination = target.position;
                agent.isStopped = false;
            }
        }
        else if (info.IsName("IDLE")) agent.isStopped = true;
        else if (info.IsName("SLAM")) agent.isStopped = true;
    }

    void Hit(string[] args)
    {
        animator.SetTrigger("hit");
    }
}
