using System;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static event Action<Vector2> OnMadeNoise;

    [SerializeField] private PlayerController player;

    public void MakeNoiseByRunning()
    {
        OnMadeNoise.Invoke(player.transform.position);
    }

    public void MakeNoiseByThrowing(Vector2 targetPosition)
    {
        OnMadeNoise.Invoke(targetPosition);
    }
}
