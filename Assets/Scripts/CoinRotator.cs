using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100.0f;

    [Header("Bobbing Settings")]
    public float bobSpeed = 2.0f; // Speed of up and down motion
    public float bobHeight = 0.25f; // Distance it moves up and down

    private Vector3 startPos;

    void Start()
    {
        // Store starting position so it bobs relative to place
        startPos = transform.position;
    }

    void Update()
    {
        // Vertical rotation
       transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);

        // Smooth up and down bobbing
        float newY = startPos.y +  Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}