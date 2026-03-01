using System;
using UnityEngine;

public class NoiseHandler
{
    public event Action<Vector3, float> OnMakeNoise;

    public void MakeNoise(Vector3 position, float loudness)
    {
        OnMakeNoise?.Invoke(position, loudness);
    }
}
