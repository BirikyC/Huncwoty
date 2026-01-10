using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private void Update()
    {
        if (player == null) return;

        float angle = player.GetCurrentAngle();

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
