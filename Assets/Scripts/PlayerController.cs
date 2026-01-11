using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum PlayerRotation
{
    LeftBack, LeftBackStatic,
    LeftFront, LeftFrontStatic,
    RightBack, RightBackStatic,
    RightFront, RightFrontStatic
}

public class PlayerController : MonoBehaviour
{
    public event Action OnDied;

    private Rigidbody2D rb;
    private Vector2 input;
    
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    private PlayerRotation currentRotation;
    private float currentAngle;

    [SerializeField] private NoiseManager noiseManager;

    private bool isFreezedMovement = false;
    private bool isSprinting = false;
    private bool isMoving = false;

    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private DirectionArrowController arrowController;
    [SerializeField] private float arrowDelay = 30.0f;

    private List<CanController> canList = new List<CanController>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        arrowController.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (isFreezedMovement) return;
        
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mouseWorldPosition.z = 0f;

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        UpdateCurrentRotationFromAngle();

        rb.linearVelocity = input * (isSprinting ? sprintSpeed : speed);

        if (isSprinting)
        {
            noiseManager.MakeNoiseByRunning();
        }

        if(gameTimer.GetPastTime() >= arrowDelay && !arrowController.IsActive())
        {
            arrowController.Show();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            OnDied.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        if(context.started) isMoving = true;
        else if(context.canceled) isMoving = false;
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

        ThrowCan();
    }

    private void UpdateCurrentRotationFromAngle()
    {
        float angle = (currentAngle + 360f) % 360f;

        if (angle >= 0.0f && angle < 90.0f)
            currentRotation = isMoving ? PlayerRotation.RightBack : PlayerRotation.RightBackStatic;
        else if (angle >= 90.0f && angle < 180.0f)
            currentRotation = isMoving ? PlayerRotation.LeftBack : PlayerRotation.LeftBackStatic;
        else if (angle >= 180.0f && angle < 270.0f)
            currentRotation = isMoving ? PlayerRotation.LeftFront : PlayerRotation.LeftFrontStatic;
        else if (angle >= 270.0f && angle < 360.0f)
            currentRotation = isMoving ? PlayerRotation.RightFront : PlayerRotation.RightFrontStatic;
    }

    public void ToggleFreezeMovement(bool isFreezed)
    {
        isFreezedMovement = isFreezed;

        if(isFreezedMovement)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void ThrowCan()
    {
        if (canList.Count == 0)
            return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0f;

        CanController can = canList[canList.Count - 1];
        canList.RemoveAt(canList.Count - 1);

        can.transform.position = transform.position + new Vector3(0f, 1f, 0f);
        can.Throw(can.transform.position, mouseWorldPosition);

        noiseManager.MakeNoiseByThrowing(mouseWorldPosition);
    }

    public void AddCan(CanController can)
    {
        canList.Add(can);
    }

    public Vector2 GetDirection()
    {
        float angleRad = currentAngle * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        return direction.normalized;
    }

    public PlayerRotation GetCurrentRotation()
    {
        return currentRotation;
    }

    public float GetCurrentAngle()
    {
        return currentAngle;
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
