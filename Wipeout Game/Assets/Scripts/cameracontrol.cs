using UnityEngine;

public class tCameraController : MonoBehaviour
{
    [Header("Camera Mode")]
    public bool isFirstPerson = true;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    [Header("First Person")]
    public float eyeHeight = 1.7f;

    [Header("Third Person")]
    public float followDistance = 6f;
    public float followHeight = 2f;
    public float cameraSmoothSpeed = 5f;

    [Header("References")]
    public Transform playerBody; // Assign in inspector

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private Vector3 firstPersonLocalPos;

    void Start()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Setup references
        if (playerBody == null)
        {
            playerBody = transform.parent;
        }

        if (playerBody == null)
        {
            Debug.LogError("NoFloatCameraController: Player Body must be assigned!");
            return;
        }

        // Store initial rotations
        horizontalRotation = playerBody.eulerAngles.y;

        // Setup first person position
        firstPersonLocalPos = new Vector3(0, eyeHeight, 0);

        // Set initial camera mode
        SetCameraMode();
    }

    void Update()
    {
        if (playerBody == null) return;

        HandleCameraToggle();
        HandleMouseLook();
        HandleCursorToggle();
    }

    void LateUpdate()
    {
        if (playerBody == null) return;

        HandleCameraPosition();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Accumulate rotations (most stable method)
        horizontalRotation += mouseX;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

        if (isFirstPerson)
        {
            // First person: Player rotates horizontally, camera vertically
            playerBody.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
            transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
        else
        {
            // Third person: Player rotates horizontally only
            playerBody.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
            // Camera rotation handled in HandleCameraPosition
        }
    }

    void HandleCameraToggle()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            verticalRotation = 0f; // Reset vertical look
            SetCameraMode();
        }
    }

    void SetCameraMode()
    {
        if (isFirstPerson)
        {
            // First person: Set as child with local position
            transform.parent = playerBody;
            transform.localPosition = firstPersonLocalPos;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            // Third person: Remove from parent for free positioning
            transform.parent = null;
        }
    }

    void HandleCameraPosition()
    {
        if (!isFirstPerson)
        {
            // Third person camera positioning
            Vector3 targetPos = playerBody.position
                - playerBody.forward * followDistance
                + Vector3.up * followHeight;

            // Smooth movement
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * cameraSmoothSpeed
            );

            // Look at player with vertical rotation offset
            Vector3 lookTarget = playerBody.position + Vector3.up * eyeHeight;
            Vector3 direction = (lookTarget - transform.position).normalized;

            // Apply vertical rotation
            Quaternion baseRotation = Quaternion.LookRotation(direction);
            Quaternion verticalOffset = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.rotation = baseRotation * verticalOffset;
        }
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}