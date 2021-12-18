using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class FiniteEnemy : MonoBehaviour
{
    private Animator animator;

    private Ray ray;
    private RaycastHit hit;
    private AnimatorStateInfo info;
    private string objectInSite;

    private CharacterController controller;

    // For dot product of view
    public Vector3 direction;
    public bool inFOV; // FOV = Field of View
    public bool objectBetweenTargetAndSelf = true;

    [SerializeField] private float speed = 1;
    [SerializeField] private Transform head = null;

    private Transform target;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        target = PlayerMovementScript.instance.gameObject.transform;
    }

    void Update()
    {
        direction = (target.position - transform.position).normalized;
        inFOV = Vector3.Dot(transform.forward.normalized, direction) > .7f;

        Debug.DrawRay(transform.position, direction * 100, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red);

        if(Physics.Raycast(transform.position, direction * 100, out hit))
        {
            if (hit.collider.gameObject.tag == "Player") objectBetweenTargetAndSelf = false;
            else objectBetweenTargetAndSelf = true;
        }

        if (!objectBetweenTargetAndSelf && inFOV)
        {
            animator.SetBool("canSeePlayer", true);
            head.LookAt(target);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }
        else animator.SetBool("canSeePlayer", false);

        if (info.IsName("CHASE"))
        {
            
            if (objectInSite != "Player") animator.SetBool("canSeePlayer", false);
            else controller.SimpleMove(Vector3.forward * Time.deltaTime * speed); //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
