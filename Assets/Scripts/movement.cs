using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ClickToLookWalkerWithMinimap : MonoBehaviour
{
    [Header("Movement")]
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.8f; // how much faster when sprinting
    public float jumpForce = 5f;
    public float gravity = 9.81f;


    [Header("Mouse Look")]
    float cameraPitch = 0.0f;
    [SerializeField] Transform playerCamera = null;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    public float mouseSensitivity = 2f;
    public Transform head;
    // private bool isLooking = false;
    private float rotationX = 0f;
    private float rotationY = 0f;
    bool lockCursor = true;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    [Header("Look Limits")]
    public float minPitch = -15f; // look all the way down
    public float maxPitch = 85f;  // limit looking up


    [Header("Head Effects")]
    public float bobbingAmplitude = 0.05f;
    public float bobbingFrequency = 6f;
    public float idleSwayAmplitude = 0.02f;
    public float idleSwayFrequency = 1.5f;

    [Header("Minimap")]
    public Camera minimapCamera;
    public Transform minimapQuad;        // <-- ✅ missing in your version
    private Vector3 smallScale;
    public Vector3 bigScale = new Vector3(0.4f, 0.4f, 0.4f); // only set how big it should get
    private bool isMinimapBig = false;

    [Header("Minimap Dot")]
    public Transform minimapDot;

    private CharacterController controller;
    private Vector3 headStartPos;
    private float bobbingTimer;
    private float idleTimer;
    private float verticalVelocity;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController>();

        if (!head || !minimapCamera || !minimapDot || !minimapQuad)
        {
            Debug.LogError("Please assign all references in the Inspector!");
            enabled = false;
            return;
        }

        headStartPos = head.localPosition;

        Vector3 initialRotation = transform.rotation.eulerAngles;
        smallScale = minimapQuad.localScale; // capture whatever you set in the scene
        rotationY = initialRotation.y;
        rotationX = head.localEulerAngles.x;
    }

    void Update()
    {
        // --- Mouse Look ---
        UpdateMouseLook();
        // if (Input.GetMouseButtonDown(0)) isLooking = true;
        // if (Input.GetMouseButtonUp(0)) isLooking = false;

        // if (isLooking)
        // {
        //     float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        //     float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //     rotationY += mouseX;
        //     rotationX -= mouseY;
        //     rotationX = Mathf.Clamp(rotationX, minPitch, maxPitch);

        //     transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        //     head.localRotation = Quaternion.Euler(rotationX, 0, 0);
        // }

        // --- Movement ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveInput = new Vector3(h, 0, v);

        // --- Sprinting ---
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && moveInput.magnitude > 0.1f)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 move = transform.TransformDirection(moveInput.normalized) * currentSpeed;


        // --- Jump ---
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            if (Input.GetButtonDown("Jump")) verticalVelocity = jumpForce;
        }
        else verticalVelocity -= gravity * Time.deltaTime;

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);

        // --- Head Bobbing / Idle Sway ---
        if (moveInput.magnitude > 0.1f)
        {
            bobbingTimer += Time.deltaTime * bobbingFrequency;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * bobbingAmplitude;
            head.localPosition = headStartPos + new Vector3(0, bobbingOffset, 0);
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime * idleSwayFrequency;
            float idleOffset = Mathf.Sin(idleTimer) * idleSwayAmplitude;
            head.localPosition = Vector3.Lerp(head.localPosition, headStartPos + new Vector3(0, idleOffset, 0), Time.deltaTime * 2f);
            bobbingTimer = 0f;
        }

        // --- Minimap Zoom ---
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMinimapBig = !isMinimapBig;
            minimapQuad.localScale = isMinimapBig ? bigScale : smallScale;
        }

        // --- Dot Tracking ---
        minimapDot.position = new Vector3(transform.position.x, minimapDot.position.y, transform.position.z);

        // --- Minimap Camera Follow ---
        minimapCamera.transform.position = new Vector3(transform.position.x, minimapCamera.transform.position.y, transform.position.z);
    }

    void UpdateMouseLook()
    {
        Vector2 targetMosueDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMosueDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector2.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
}
