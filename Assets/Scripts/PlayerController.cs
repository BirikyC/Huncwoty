using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 input;
    [SerializeField] private float speed = 5.0f;

    [SerializeField] private NoiseManager noiseManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        noiseManager.MakeNoise(transform.position, 5.0f);
        Debug.Log(1);
    }
}
