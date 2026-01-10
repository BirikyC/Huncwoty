using UnityEngine;

public class DirectionArrowController : MonoBehaviour
{
    [SerializeField] private GoalTrigger target;

    private void Update()
    {
        if (target == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle += 180f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
