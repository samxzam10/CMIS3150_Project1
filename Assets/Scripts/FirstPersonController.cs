using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene restart
using TMPro; 

public class FirstPersonController : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Movement Settings")]
    public float walkSpeed = 5.0f;
    public float runSpeed = 9.0f;
    public float jumpForce = 6.0f;

    [Header("Look Settings")]
    public float mouseSensitivity = 200.0f;
    public Transform cameraTransform;

    [Header("Game State")]
    public int lives = 3;
    private int coinsCollected = 0;
    private const int totalCoins = 8;

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateUI();

        // Make sure game over / win panels start hidden
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        // Lock mouse to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // MOUSE LOOK LOGIC
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseX);

        // JUMP LOGIC
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // WASD MOVEMENT LOGIC
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 newPos = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
        
        rb.MovePosition(newPos);
    }

    // SOLID COLLISIONS (Ground & Obstacles)
    private void OnCollisionEnter(Collision collision)
    {
        // Print every solid object you collide with to the Console window
        Debug.Log("COLLIDED WITH: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            lives--;
            UpdateUI();

            if (lives <= 0)
            {
                TriggerGameOver();
            }
        }
    }

    // TRIGGER COLLISIONS (Coin Collectibles)
    private void OnTriggerEnter(Collider other)
    {
        // Print every trigger object you walk through to the Console window
        Debug.Log("TOUCHED TRIGGER: " + other.gameObject.name);

        if (other.CompareTag("Coin"))
        {
            coinsCollected++;
            UpdateUI();
            Destroy(other.gameObject);

            if (coinsCollected >= totalCoins)
            {
                TriggerWin();
            }
        }
    }

    private void UpdateUI()
    {
        if (coinText != null) coinText.text = "Coins: " + coinsCollected + " / " + totalCoins;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }

    private void TriggerGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        EndGame();
    }

    private void TriggerWin()
    {
        if (winPanel != null) winPanel.SetActive(true);
        EndGame();
    }

    private void EndGame()
    {
        Time.timeScale = 0f; // Pause physics & gameplay
        Cursor.lockState = CursorLockMode.None; // Unlock cursor for UI
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Restore game speed before reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}