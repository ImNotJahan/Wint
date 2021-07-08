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

    public bool disabled = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
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
}