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

    [SerializeField] private float speed = 1;
    [SerializeField] private Transform head = null;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ray.origin = head.position;
        ray.direction = head.forward;
        info = animator.GetCurrentAnimatorStateInfo(0);
        objectInSite = "";

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            objectInSite = hit.collider.gameObject.tag;
            if(objectInSite == "Player")
            {
                animator.SetBool("canSeePlayer", true);
            }
        }

        if (info.IsName("CHASE"))
        {
            Transform target = PlayerMovementScript.instance.gameObject.transform;
            head.LookAt(target);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            if (objectInSite != "Player") animator.SetBool("canSeePlayer", false);
            else controller.SimpleMove(Vector3.forward * Time.deltaTime * speed); //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
