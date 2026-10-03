using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 4.0f;
    public float speed = 2.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate smooth ping-pong displacement along the X-axis
        float offset = Mathf.PingPong(Time.time * speed, moveDistance) - (moveDistance / 2f);
        transform.position = startPos + new Vector3(offset, 0f, 0f);
    }
}