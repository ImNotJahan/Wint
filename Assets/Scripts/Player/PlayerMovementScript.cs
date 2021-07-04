using UnityEngine;

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

    float hp = 100;
    
    bool bleeding = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        currentSpeed = speed;

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * 2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded && Time.timeScale != 0.5f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }
}