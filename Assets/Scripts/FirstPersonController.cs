using UnityEngine;
using TMPro; 

public class FirstPersonController : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;

    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 9.0f;
    public float jumpForce = 6.0f;

    [Header("Look Settings")]
    public float mouseSensitivity = 200.0f;
    public Transform cameraTransform; // Drag Main Camera here in the Inspector

    [Header("Game State")]
    public int lives = 3; // Start with 3 lives
    private int coinsCollected = 0;
    private const int totalCoins = 8; // Need 8 coins to win

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f; // Stores up/down camera angle

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateUI();

        // Lock mouse to the middle of screen so cursor doesn't drift off-game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // MOUSE LOOK LOGIC
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Look Up/Down: Clamp angle between -90 and 90
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Look Left/Right: Turn entire player body horizontally
        transform.Rotate(Vector3.up * mouseX);

        // JUMP LOGIC
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevent double jumping
        }
    }

    void FixedUpdate()
    {
        // WASD MOVEMENT LOGIC
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Hold Left Shift to run, otherwise walk
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPos = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
        
        rb.MovePosition(newPos);
    }

    // SOLID COLLISIONS (Ground & Obstacles)    
    private void OnCollisionEnter(Collision collision)
    {
        // Reset jump ability when landing back on ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Subtract life on physical contact with red obstacles
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            lives--;
            UpdateUI(); // Update text on screen!
            Debug.Log("Hit Obstacle! Lives remaining: {lives}");

            if (lives <= 0)
            {
                Debug.Log("Game Over!");
                Application.Quit();
            }
        }
    }

    // TRIGGER COLLISIONS (Coin Collectibles)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinsCollected++;
            UpdateUI(); // Update text on screen!
            Destroy(other.gameObject);
            Debug.Log("Collected Coin! ({coinsCollected}/{totalCoins})");

            if (coinsCollected >= totalCoins)
            {
                Debug.Log("You Win!");
                Application.Quit();
            }
        }
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: {coinsCollected} / {totalCoins}";
        }
        if (livesText != null)
        {
            livesText.text = "Lives: {lives}";
        }
    }
}