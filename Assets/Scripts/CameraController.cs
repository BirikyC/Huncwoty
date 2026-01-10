using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float speed = 5.0f;

    [SerializeField] private float zoomStep = 0.5f;
    [SerializeField] private float zoomSpeed = 4.0f;
    private float targetZoom;

    private void Start()
    {
        const float START_ZOOM = 7.0f;

        Camera.main.orthographicSize = START_ZOOM;
        targetZoom = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        // Pozycja Kamery
        Vector3 targetPosition = player.transform.position + offset;

        Vector3 currentPosition = Vector3.Lerp(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        transform.position = currentPosition;

        // Zoom Kamery
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }

    public void OnZoomIn(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        const float MIN_ZOOM = 5.0f;

        targetZoom = Mathf.Max(MIN_ZOOM, targetZoom - zoomStep);
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        if(!context.started) return;

        const float MAX_ZOOM = 9.0f;

        targetZoom = Mathf.Min(MAX_ZOOM, targetZoom + zoomStep);
    }
}
