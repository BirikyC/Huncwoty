using System;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static event Action<Vector2> OnMadeNoise;

    [SerializeField] private PlayerController player;
    [SerializeField] private float throwRadius = 4.0f;

    public void MakeNoiseByRunning()
    {
        OnMadeNoise.Invoke(player.transform.position);
    }

    public void MakeNoiseByThrowing()
    {
        Vector3 direction = player.GetDirection();

        Vector2 noisePosition = player.transform.position + (direction * throwRadius);

        OnMadeNoise.Invoke(noisePosition);
    }
}
