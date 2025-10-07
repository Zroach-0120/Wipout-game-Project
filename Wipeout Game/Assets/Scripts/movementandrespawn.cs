using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera playerCamera;

    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    [Header("Respawn")]
    public Transform respawnPoint;

    private int jumpCount = 0;
    private int maxJumps = 2;
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = false;

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            Debug.LogError("No camera found! Make sure camera is a child of the player.");

        Cursor.lockState = CursorLockMode.Locked;

        if (respawnPoint == null)
        {
            GameObject start = new GameObject("RespawnPoint");
            start.transform.position = transform.position;
            start.transform.rotation = transform.rotation;
            respawnPoint = start.transform;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleJump();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (playerCamera != null)
        {
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            jumpCount = 0;

        if (collision.gameObject.CompareTag("Hazard"))
            Respawn();
    }

    void Respawn()
    {
        // Teleport player safely without breaking movement
        rb.isKinematic = true;
        rb.MovePosition(respawnPoint.position);
        rb.MoveRotation(respawnPoint.rotation);
        rb.isKinematic = false;

        // Optional: reset jumps
        jumpCount = 0;
    }

}
