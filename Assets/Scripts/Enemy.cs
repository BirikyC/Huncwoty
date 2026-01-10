using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    private Vector2 focus_pos;
    private bool chase = false;

    float speed = 1.0f;

    static Tilemap tilemap;

    [SerializeField] private NoiseManager noiseManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        noiseManager.addEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (chase) 
        {
            Debug.Log(chase);
            Vector2 dir = (focus_pos - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + dir * speed * Time.deltaTime;
        }
    }

    public void Notify(Vector2 pos)
    {
        Debug.Log("ok?");
        focus_pos = pos;
        chase = true;
    }
}
