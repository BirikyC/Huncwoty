using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float avoidDistance = 1.0f;
    [SerializeField] private float obstacleCheckDistance = 0.8f;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float hearNoiseRadius = 5.0f;

    private bool isChasing = false;
    private bool isAvoidingObstacle = false;
    private Vector2 avoidDirection;
    private Vector3 targetPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        NoiseManager.OnMadeNoise += HandleNoise;
    }

    private void OnDisable()
    {
        NoiseManager.OnMadeNoise -= HandleNoise;
    }

    private void FixedUpdate()
    {
        if (!isChasing) return;

        Vector2 toTarget = (targetPosition - transform.position).normalized;

        if (!isAvoidingObstacle)
        {
            if (IsObstacleAhead(toTarget))
            {
                StartAvoiding(toTarget);
            }
            else
            {
                rb.linearVelocity = toTarget * speed;
            }
        }
        else
        {
            rb.linearVelocity = avoidDirection * speed;

            if (!IsObstacleAhead(toTarget))
            {
                isAvoidingObstacle = false;
            }
        }

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            Stop();
        }
    }


    private void HandleNoise(Vector2 noisePosition)
    {
        if (Vector2.Distance(transform.position, noisePosition) > hearNoiseRadius) return;

        targetPosition = noisePosition;
        isChasing = true;
    }

    private void Stop()
    {
        isChasing = false;
        rb.linearVelocity = Vector2.zero;
    }

    private bool IsObstacleAhead(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            obstacleCheckDistance,
            obstacleLayer
        );

        return hit.collider != null;
    }

    private void StartAvoiding(Vector2 toTarget)
    {
        isAvoidingObstacle = true;

        Vector2 left = new Vector2(-toTarget.y, toTarget.x);
        Vector2 right = new Vector2(toTarget.y, -toTarget.x);

        float leftScore = Vector2.Distance(
            (Vector2)transform.position + left,
            targetPosition
        );

        float rightScore = Vector2.Distance(
            (Vector2)transform.position + right,
            targetPosition
        );

        avoidDirection = leftScore < rightScore ? left : right;
    }

}
