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

    List<Vector2Int> tiles = null;
    int tile_id = 0;

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
            //Vector2 dir = (focus_pos - (Vector2)transform.position).normalized;
            //transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;

            Vector2 worldPos = tilemap.CellToWorld(new Vector3Int(tiles[tile_id].x, tiles[tile_id].y, 0));
            Vector2 dir = (worldPos - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;

            //Debug.Log(tiles[tile_id]);
            if ((worldPos - (Vector2)transform.position).normalized != dir)
            {
                //Debug.Log(tilemap.WorldToCell(transform.position) + " w");
                ++tile_id;
                chase = tile_id < tiles.Count;
            }
        }
    }

    public void HandleNoise(Vector2 pos)
    {
        float dist = Vector2.Distance(transform.position, pos);
        
        if (dist > hearNoiseRadius) return;

        focus_pos = pos;
        chase = true;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (focus_pos - (Vector2)transform.position).normalized, dist, PathFInd.OBSTACLE_MASK);

        //Debug.Log("Start: " + (Vector2Int)tilemap.WorldToCell(transform.position));

        tiles = PathFInd.FindPath((Vector2Int)tilemap.WorldToCell(transform.position), (Vector2Int)tilemap.WorldToCell(focus_pos), tilemap);
        if (tiles == null) chase = false; 
        else tile_id = 0;

        Debug.Log(tiles == null);

        if (tiles != null)
        {
            foreach (Vector2Int v in tiles) Debug.Log(v);
            Debug.Log("");

            Color c = Color.green;
            foreach (Vector2Int v in tiles)
            {
                Vector3 w = tilemap.GetCellCenterWorld((Vector3Int)v);
                Debug.DrawLine(w, Vector3.one * 0.3f, c);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (tiles == null) return;

        Gizmos.color = Color.green;
        foreach (Vector2Int c in tiles)
        {
            Vector3 w = tilemap.GetCellCenterWorld((Vector3Int)c);
            Gizmos.DrawWireCube(w, Vector3.one * 0.3f);
        }
    }
}
