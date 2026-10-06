using UnityEngine;

// Controls the automated back-and-forth horizontal movement for obstacle hazard objects
public class MovingObstacle : MonoBehaviour
{
    // --- INSPECTOR FIELDS ---

    [Header("Movement Settings")]
    // Maximum distance back and forth the obstacle moves along its axis
    public float moveDistance = 4.0f;
    // Speed multiplier controlling how fast the obstacle translates
    public float speed = 2.0f;

    // --- PRIVATE VARIABLES ---

    // Stores the initial world position of the obstacle at scene start
    private Vector3 startPos;

    // Start is called once before the first frame update
    void Start()
    {
        // Record starting origin position so movement oscillates evenly around it
        startPos = transform.position;
    }

    // Update is called once per frame to recalculate position
    void Update()
    {
        // Calculate smooth ping-pong displacement back and forth along the X-axis:
        // 1. Mathf.PingPong calculates a value bouncing between 0 and moveDistance based on game time * speed.
        // 2. Subtracting (moveDistance / 2f) shifts the oscillation range so it moves symmetrically around startPos.
        float offset = Mathf.PingPong(Time.time * speed, moveDistance) - (moveDistance / 2f);

        // Apply calculated offset position along the local X-axis relative to starting position
        transform.position = startPos + new Vector3(offset, 0f, 0f);
    }
}