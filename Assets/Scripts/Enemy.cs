using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private Vector2 focus_pos;
    private bool chase = false;

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float hearNoiseRadius = 5.0f;

    static Tilemap tilemap;

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
            Vector2 dir = (focus_pos - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;
        }
    }

    public void Notify(Vector2 pos)
    {
        focus_pos = pos;
        chase = true;
    }

    private void HandleNoise(Vector2 noisePosition)
    {
        if (Vector2.Distance(transform.position, noisePosition) > hearNoiseRadius) return;

        Debug.Log(noisePosition);
    }
}
