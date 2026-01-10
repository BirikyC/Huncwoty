using UnityEngine;
using System;

public class EnemyCheckForCollisions : MonoBehaviour
{
    public event Action OnObstacleInFront;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            OnObstacleInFront.Invoke();
        }
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
