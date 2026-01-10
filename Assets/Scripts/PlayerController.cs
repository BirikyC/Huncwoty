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
    [SerializeField] private float rotationSpeed = 10.0f;
    private PlayerRotation rotation;
    private float currentAngle;

    [SerializeField] private NoiseManager noiseManager;

    private bool isFreezedMovement = false;
    private bool isSprinting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isFreezedMovement)
        {
            rb.linearVelocity = input * (isSprinting ? sprintSpeed : speed);

            float targetAngle = GetRotationAngle();

            currentAngle = Mathf.LerpAngle(
                currentAngle,
                targetAngle,
                rotationSpeed * Time.fixedDeltaTime
            );

            rb.MoveRotation(currentAngle);

            if (isSprinting)
            {
                noiseManager.MakeNoiseByRunning();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        bool isUp = input.y > 0.5f;
        bool isRight = input.x > 0.5f;
        bool isDown = input.y < -0.5f;
        bool isLeft = input.x < -0.5f;

        if (isUp && isRight)
            rotation = PlayerRotation.UpRight;
        else if (isRight && isDown)
            rotation = PlayerRotation.RightDown;
        else if (isDown && isLeft)
            rotation = PlayerRotation.DownLeft;
        else if (isLeft && isUp)
            rotation = PlayerRotation.LeftUp;
        else if (isUp)
            rotation = PlayerRotation.Up;
        else if (isRight)
            rotation = PlayerRotation.Right;
        else if (isDown)
            rotation = PlayerRotation.Down;
        else if (isLeft)
            rotation = PlayerRotation.Left;
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

    private float GetRotationAngle()
    {
        return rotation switch
        {
            PlayerRotation.Right => 0f,
            PlayerRotation.UpRight => 45f,
            PlayerRotation.Up => 90f,
            PlayerRotation.LeftUp => 135f,
            PlayerRotation.Left => 180f,
            PlayerRotation.DownLeft => 225f,
            PlayerRotation.Down => 270f,
            PlayerRotation.RightDown => 315f,
            _ => 0f
        };
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
}
