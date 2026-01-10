using System.Collections;
using UnityEngine;

public class CanController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteGround;
    [SerializeField] private Sprite spriteThrow;

    [SerializeField] private float maxRange = 10.0f;
    [SerializeField] private float throwDuration = 0.6f;
    [SerializeField] private float throwHeight = 1.5f;

    [SerializeField] private float pickupDelay = 1.0f;
    private bool canBePickedup = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = spriteGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            Take(player); ;
        }
    }

    private void Take(PlayerController player)
    {
        if (!canBePickedup) return;

        gameObject.SetActive(false);

        player.AddCan(this);
    }

    public void Throw(Vector2 startPosition, Vector2 targetPosition)
    {
        gameObject.SetActive(true);
        spriteRenderer.sprite = spriteThrow;

        canBePickedup = false;

        StopAllCoroutines();
        StartCoroutine(ThrowCoroutine(startPosition, targetPosition));
    }

    private IEnumerator ThrowCoroutine(Vector2 startPosition, Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - startPosition);
        if (direction.magnitude > maxRange)
        {
            direction = direction.normalized * maxRange;
        }

        Vector2 endPos = startPosition + direction;

        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime / throwDuration;

            Vector2 currentPosition = Vector2.Lerp(startPosition, endPos, time);

            float height = 4f * throwHeight * time * (1f - time);
            currentPosition.y += height;

            transform.position = currentPosition;

            yield return null;
        }

        transform.position = endPos;
        spriteRenderer.sprite = spriteGround;

        yield return new WaitForSeconds(pickupDelay);
        canBePickedup = true;
    }
}
