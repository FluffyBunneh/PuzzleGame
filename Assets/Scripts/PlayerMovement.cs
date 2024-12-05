using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public bool invertY = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private bool canMove = true; // Flag to determine if the player can move

    private void Start()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // If movement is allowed, handle it
        if (canMove)
        {
            HandleMouseLook();
            HandleMovement();
            HandleJump();
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= (invertY ? -1 : 1) * mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    // Method to disable movement
    public void DisableMovement()
    {
        canMove = false; // Set the flag to false to stop movement
    }

    // Method to enable movement
    public void EnableMovement()
    {
        canMove = true; // Set the flag to true to allow movement
    }
}
