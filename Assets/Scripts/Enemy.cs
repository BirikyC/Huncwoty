using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private Vector2 focus_pos;
    private bool chase = false;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float partolSpeed = 3.0f;
    [SerializeField] private float hearNoiseRadius = 5.0f;

    [SerializeField] private float afterChaseCooldown = 1.0f;
    private float cooldownTimer = 0f;
    private bool inCooldown = false;

    public Tilemap tilemap;
    [SerializeField] private Transform[] defaultPath;
    private int currentPointOnDefaultPath = 0;

    List<Vector2> points = null;
    int point_id = 0;

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
        if (inCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                inCooldown = false;
            }
            return;
        }

        if (chase)
        {
            ChaseMovement();
        }
        else
        {
            PatrolMovement();
        }
    }

    void ChaseMovement()
    {
        if (points == null || point_id >= points.Count)
        {
            chase = false;
            StartCooldown();
            return;
        }

        Vector2 target = points[point_id];
        Vector2 pos = transform.position;

        Vector2 dir = (target - pos).normalized;
        transform.position = pos + dir * speed * Time.deltaTime;

        float dist = Vector2.Distance(transform.position, target);

        if (dist < 0.1f)
        {
            point_id++;
            if (point_id >= points.Count)
            {
                chase = false;
                StartCooldown();
            }
        }
    }


    void PatrolMovement()
    {
        if (defaultPath == null || defaultPath.Length == 0)
            return;

        Transform target = defaultPath[currentPointOnDefaultPath];
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;

        transform.position = (Vector2)transform.position + dir * partolSpeed * Time.deltaTime;

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist < 0.1f) // dotar³ do punktu
        {
            currentPointOnDefaultPath = (currentPointOnDefaultPath + 1) % defaultPath.Length;
        }
    }

    public void HandleNoise(Vector2 pos)
    {
        inCooldown = false;
        float dist = Vector2.Distance(transform.position, pos);

        if (dist > hearNoiseRadius) return;

        chase = false;

        focus_pos = pos;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (focus_pos - (Vector2)transform.position).normalized, dist, PathFInd.OBSTACLE_MASK);

        Gizmos.color = Color.green;

        if (!hit)
        {
            points = new List<Vector2> { pos };
            //Debug.Log(PathFInd.OBSTACLE_MASK);
            chase = true;
            point_id = 0;
            return;
        }

        points = PathFInd.FindPath(transform.position, focus_pos, tilemap);

        if (points == null) chase = false;
        else
        {
            point_id = 0;
            chase = true;
            currentPointOnDefaultPath = 0;
        }
    }

    void OnDrawGizmos()
    {
        if (points == null) return;

        Gizmos.color = Color.green;

        if (points.Count == 1)
        {
            Gizmos.DrawLine(transform.position, points[0]);
            return;
        }

        foreach (Vector2 c in points)
        {
            Gizmos.DrawWireCube(c, Vector3.one * 0.3f);
        }
    }

    void StartCooldown()
    {
        inCooldown = true;
        cooldownTimer = afterChaseCooldown;
    }
}