using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerRotation
{
    Up, UpRight,
    Right, RightDown,
    Down, DownLeft,
    Left, LeftUp
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 input;
    
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    private PlayerRotation currentRotation;
    private float currentAngle;

    [SerializeField] private NoiseManager noiseManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite upRightSprite;
    [SerializeField] private Sprite rightSprite;
    [SerializeField] private Sprite rightDownSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite downLeftSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite leftUpSprite;

    private bool isFreezedMovement = false;
    private bool isSprinting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isFreezedMovement) return;
        
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Debug.Log(mouseWorldPosition);

        mouseWorldPosition.z = 0f;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        UpdateCurrentRotationFromAngle();
        UpdateSpriteByRotation();

        rb.linearVelocity = input * (isSprinting ? sprintSpeed : speed);

        if (isSprinting)
        {
            noiseManager.MakeNoiseByRunning();
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Throw();
    }

    private void UpdateSpriteByRotation()
    {
        if (currentRotation == PlayerRotation.Up)
            spriteRenderer.sprite = upSprite;
        else if (currentRotation == PlayerRotation.UpRight)
            spriteRenderer.sprite = upRightSprite;
        else if (currentRotation == PlayerRotation.Right)
            spriteRenderer.sprite = rightSprite;
        else if (currentRotation == PlayerRotation.RightDown)
            spriteRenderer.sprite = rightDownSprite;
        else if (currentRotation == PlayerRotation.Down)
            spriteRenderer.sprite = downSprite;
        else if (currentRotation == PlayerRotation.DownLeft)
            spriteRenderer.sprite = downLeftSprite;
        else if (currentRotation == PlayerRotation.Left)
            spriteRenderer.sprite = leftSprite;
        else if (currentRotation == PlayerRotation.LeftUp)
            spriteRenderer.sprite = leftUpSprite;
    }

    private void UpdateCurrentRotationFromAngle()
    {
        float angle = (currentAngle + 360f) % 360f;

        if (angle >= 337.5f || angle < 22.5f)
            currentRotation = PlayerRotation.Right;
        else if (angle >= 22.5f && angle < 67.5f)
            currentRotation = PlayerRotation.UpRight;
        else if (angle >= 67.5f && angle < 112.5f)
            currentRotation = PlayerRotation.Up;
        else if (angle >= 112.5f && angle < 157.5f)
            currentRotation = PlayerRotation.LeftUp;
        else if (angle >= 157.5f && angle < 202.5f)
            currentRotation = PlayerRotation.Left;
        else if (angle >= 202.5f && angle < 247.5f)
            currentRotation = PlayerRotation.DownLeft;
        else if (angle >= 247.5f && angle < 292.5f)
            currentRotation = PlayerRotation.Down;
        else if (angle >= 292.5f && angle < 337.5f)
            currentRotation = PlayerRotation.RightDown;
    }

    public void ToggleFreezeMovement(bool isFreezed)
    {
        isFreezedMovement = isFreezed;

        if(isFreezedMovement)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Throw()
    {
        noiseManager.MakeNoiseByThrowing();
    }

    public Vector2 GetDirection()
    {
        float angleRad = currentAngle * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        return direction.normalized;
    }

    public float GetCurrentAngle()
    {
        return currentAngle;
    }
}
