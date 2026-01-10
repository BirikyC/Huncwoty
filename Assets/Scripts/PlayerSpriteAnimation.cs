using UnityEngine;

public class PlayerSpriteAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] leftBackFrames;
    [SerializeField] private Sprite[] leftBackStaticFrames;
    [SerializeField] private Sprite[] leftFrontFrames;
    [SerializeField] private Sprite[] leftFrontStaticFrames;
    [SerializeField] private Sprite[] rightBackFrames;
    [SerializeField] private Sprite[] rightBackStaticFrames;
    [SerializeField] private Sprite[] rightFrontFrames;
    [SerializeField] private Sprite[] rightFrontStaticFrames;
    [SerializeField] private Sprite[] idleFrames;

    [SerializeField] private float frameRate = 4;
    private int currentFrame = 0;
    private float timer = 0f;

    private PlayerRotation lastRotation;
    private Sprite[] currentFrames;

    void Update()
    {
        if (frameRate <= 0) return;

        PlayerRotation currentRotation = player.GetCurrentRotation();

        if (currentRotation != lastRotation)
        {
            lastRotation = currentRotation;
            currentFrame = 0;
            timer = 0f;
            currentFrames = GetFramesForRotation(currentRotation);
            spriteRenderer.sprite = currentFrames.Length > 0 ? currentFrames[0] : null;
            return;
        }

        if (currentFrames == null || currentFrames.Length == 0)
            currentFrames = GetFramesForRotation(currentRotation);

        float frameTime = 1f / frameRate;
        timer += Time.deltaTime;
        if (timer >= frameTime)
        {
            timer -= frameTime;
            currentFrame = (currentFrame + 1) % currentFrames.Length;
            spriteRenderer.sprite = currentFrames[currentFrame];
        }
    }

    private Sprite[] GetFramesForRotation(PlayerRotation rotation)
    {
        switch (rotation)
        {
            case PlayerRotation.LeftFront: return leftFrontFrames;
            case PlayerRotation.LeftFrontStatic: return leftFrontStaticFrames;
            case PlayerRotation.LeftBack: return leftBackFrames;
            case PlayerRotation.LeftBackStatic: return leftBackStaticFrames;
            case PlayerRotation.RightFront: return rightFrontFrames;
            case PlayerRotation.RightFrontStatic: return rightFrontStaticFrames;
            case PlayerRotation.RightBack: return rightBackFrames;
            case PlayerRotation.RightBackStatic: return rightBackStaticFrames;
            default: return idleFrames;
        }
    }
}
