using UnityEngine;

public class MonsterSpriteAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] frames;

    [SerializeField] private float frameRate = 4;
    private int currentFrame = 0;
    private float timer = 0f;

    void Update()
    {
        if (frameRate <= 0 || frames.Length <= 0) return;

        float frameTime = 1f / frameRate;
        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer -= frameTime;
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}
