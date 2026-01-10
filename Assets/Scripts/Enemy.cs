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

            if ((Vector2)transform.position == worldPos)
            {
                --tile_id;
                chase = tile_id > 0;// tiles.Count;
            }
        }
    }

    public void Notify(Vector2 pos)
    {
        focus_pos = pos;
        chase = true;

        Physics2D.Raycast(transform.position, (focus_pos - (Vector2)transform.position).normalized);

        tiles = PathFInd.FindPath((Vector2Int)tilemap.WorldToCell(transform.position), (Vector2Int)tilemap.WorldToCell(focus_pos), tilemap);
        if (tiles == null) chase = false; 
        else tile_id = tiles.Count - 1;

        Debug.Log(tiles == null);

        if (tiles != null)
        {
            foreach (Vector2Int v in tiles) Debug.Log(v);
            Debug.Log("");
        }
    }

    private void HandleNoise(Vector2 noisePosition)
    {
        if (Vector2.Distance(transform.position, noisePosition) > hearNoiseRadius) return;

        Debug.Log(noisePosition);
    }
}
