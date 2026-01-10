using UnityEngine;

public class Movable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("h");
        Transform t = collision.transform;
        Vector2 dir = (transform.position - t.position).normalized;
        transform.position = transform.position + (Vector3)dir;
    }
}
