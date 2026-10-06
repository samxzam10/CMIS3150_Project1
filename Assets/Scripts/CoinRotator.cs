using UnityEngine;

// Animates pickup coins with continuous rotation and smooth vertical bobbing
public class CoinRotator : MonoBehaviour
{
    // --- INSPECTOR FIELDS ---

    [Header("Rotation Settings")]
    // Continuous rotation speed multiplier around local axis (degrees per second)
    public float rotationSpeed = 100.0f;

    [Header("Bobbing Settings")]
    // Speed multiplier controlling how fast the coin floats up and down
    public float bobSpeed = 2.0f; 
    // Maximum height displacement distance above and below start position
    public float bobHeight = 0.25f; 

    // --- PRIVATE VARIABLES ---

    // Remembers the initial spawn position so vertical bobbing oscillates around it
    private Vector3 startPos;

    // Start is called once before the first frame update
    void Start()
    {
        // Store initial transform position as baseline origin
        startPos = transform.position;
    }

    // Update is called once per frame to update animation state
    void Update()
    {
        // --- ROTATION LOGIC ---

        // Continuously rotate coin around its local Z-axis frame-rate independently
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);

        // --- VERTICAL BOBBING LOGIC ---

        // Calculate smooth wave offset using Sine function bounded between -bobHeight and +bobHeight
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        // Apply new vertical position keeping X and Z fixed at starting location
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}