using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private Vector2 focus_pos;
    private bool chase = false;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float hearNoiseRadius = 5.0f;

    public /*static*/ Tilemap tilemap;

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
        if (chase) 
        {
            Vector2 dir = (points[point_id] - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;

            if ((points[point_id] - (Vector2)transform.position).normalized != dir) // Point overshot
            {
                ++point_id;
                chase = point_id < points.Count;
            }
        }
    }

    public void HandleNoise(Vector2 pos)
    {
        float dist = Vector2.Distance(transform.position, pos);
        
        if (dist > hearNoiseRadius) return;

        chase = false;

        focus_pos = pos;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (focus_pos - (Vector2)transform.position).normalized, dist, PathFInd.OBSTACLE_MASK);

        Gizmos.color = Color.green;

        if (!hit)
        {
            points = new List<Vector2> { pos };
            Debug.Log(PathFInd.OBSTACLE_MASK);
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
}
