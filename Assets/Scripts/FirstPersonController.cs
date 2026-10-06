using UnityEngine;

// Handles player movement, jumping, mouse look, and physics triggers/collisions
public class FirstPersonController : MonoBehaviour
{
    // --- INSPECTOR REFERENCES ---

    [Header("UI Controller Reference")]
    // Reference to the GameUIController script managing HUD and canvas overlays
    public GameUIController uiController;

    [Header("Movement Settings")]
    // Normal walking speed multiplier
    public float walkSpeed = 5.0f;
    // Sprinting speed multiplier when holding Shift
    public float runSpeed = 9.0f;
    // Impulse force applied upwards when jumping
    public float jumpForce = 6.0f;

    [Header("Look Settings")]
    // Mouse rotation sensitivity speed
    public float mouseSensitivity = 200.0f;
    // Reference to the main camera transform attached to the player
    public Transform cameraTransform;

    [Header("Game State")]
    // Starting life count for the player
    public int lives = 3;
    // Tracks current number of coins collected
    private int coinsCollected = 0;
    // Target coin count required to trigger win condition
    private const int totalCoins = 8;

    // --- PRIVATE VARIABLES ---

    // Reference to attached Rigidbody component
    private Rigidbody rb;
    // Ground state flag to prevent infinite mid-air jumps
    private bool isGrounded;
    // Accumulates vertical pitch rotation angle
    private float xRotation = 0f;

    void Start()
    {
        // Cache Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Initialize HUD text displays via UI Controller
        if (uiController != null)
        {
            uiController.UpdateCoinText(coinsCollected, totalCoins);
            uiController.UpdateLivesText(lives);
        }

        // Lock and hide mouse cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- MOUSE LOOK LOGIC ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseX);

        // --- JUMP LOGIC ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // --- WASD MOVEMENT LOGIC ---
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Shift to sprint logic
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPos = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
        
        rb.MovePosition(newPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if touching ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Check if hitting obstacle hazard
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            lives--;

            if (uiController != null)
            {
                uiController.UpdateLivesText(lives);
            }

            if (lives <= 0 && uiController != null)
            {
                uiController.TriggerGameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if picking up a coin
        if (other.CompareTag("Coin"))
        {
            coinsCollected++;
            Debug.Log("Coins Collected: " + coinsCollected);

            if (uiController != null)
            {
                uiController.UpdateCoinText(coinsCollected, totalCoins);
            }

            Destroy(other.gameObject);

            if (coinsCollected >= totalCoins && uiController != null)
            {
                uiController.TriggerWin();
            }
        }
    }
}