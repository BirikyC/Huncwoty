using UnityEngine;

public class SpriteRenderOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool isStatic = false;
    [SerializeField] private float offset = 0f;
    private const int DEFAULT_ORDER = 1000;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetOrder();
    }

    void Update()
    {
        if (isStatic) return;

        SetOrder();
    }

    private void SetOrder()
    {
        spriteRenderer.sortingOrder = DEFAULT_ORDER + Mathf.FloorToInt((-transform.position.y + offset) * 10);
    }
}
