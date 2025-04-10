using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class ClickToLookWalkerWithMinimap : MonoBehaviour
{
    public static bool IsInteractingWithMap = false; // This will be set by your map control script

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.8f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    [Header("Mouse Look")]
    float cameraPitch = 0.0f;
    [SerializeField] Transform playerCamera = null;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    public float mouseSensitivity = 2f;
    public Transform head;
    private float rotationX = 0f;
    private float rotationY = 0f;
    bool lockCursor = true;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    public static bool JournalIsOpen = false;

    [Header("Audio")]
    public AudioSource footstepsSource;
    public AudioClip footstepsClip;
    public float walkPitch = 1f;
    public float sprintPitch = 1.5f;



    [Header("Look Limits")]
    public float minPitch = -15f;
    public float maxPitch = 85f;

    [Header("Head Effects")]
    public float bobbingAmplitude = 0.05f;
    public float bobbingFrequency = 6f;
    public float idleSwayAmplitude = 0.02f;
    public float idleSwayFrequency = 1.5f;

    [Header("Minimap")]
    public Camera minimapCamera;
    public RawImage minimapImage; // ✅ RawImage in your Canvas

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

        if (!head || !minimapCamera || !minimapDot || !minimapImage)
        {
            Debug.LogError("Please assign all references in the Inspector!");
            enabled = false;
            return;
        }

        if (footstepsSource != null && footstepsClip != null)
        {
            footstepsSource.clip = footstepsClip;
            footstepsSource.loop = true;
        }



        headStartPos = head.localPosition;

        Vector3 initialRotation = transform.rotation.eulerAngles;
        rotationY = initialRotation.y;
        rotationX = head.localEulerAngles.x;
    }

    void Update()
    {
        if (IsInteractingWithMap || JournalIsOpen)
            return;

        UpdateMouseLook();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveInput = new Vector3(h, 0, v);
        bool isMoving = moveInput.magnitude > 0.1f;

        float currentSpeed = moveSpeed;

        // Sprint logic
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isMoving;
        if (isSprinting)
            currentSpeed *= sprintMultiplier;

        Vector3 move = transform.TransformDirection(moveInput.normalized) * currentSpeed;

        // Gravity and jump
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
            if (Input.GetButtonDown("Jump"))
                verticalVelocity = jumpForce;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);

        // Head bobbing and idle sway
        if (isMoving)
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

        // Footstep audio control
        if (footstepsSource != null)
        {
            if (isMoving && controller.isGrounded)
            {
                footstepsSource.pitch = isSprinting ? sprintPitch : walkPitch;
                if (!footstepsSource.isPlaying)
                    footstepsSource.Play();
            }
            else
            {
                if (footstepsSource.isPlaying)
                    footstepsSource.Stop();
            }
        }

        // Minimap tracking
        Vector3 pos = transform.position;
        minimapDot.position = new Vector3(pos.x, minimapDot.position.y, pos.z);
        minimapCamera.transform.position = new Vector3(pos.x, minimapCamera.transform.position.y, pos.z);
    }



    void UpdateMouseLook()
    {
        Vector2 targetMosueDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMosueDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        playerCamera.localEulerAngles = Vector2.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
}
