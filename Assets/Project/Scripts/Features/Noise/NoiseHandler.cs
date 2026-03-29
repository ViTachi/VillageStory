using System;
using UnityEngine;

namespace Noise
{
    public class NoiseHandler : INoiseHandler
    {
        public event Action<Vector3, float> OnMakeNoise;

        public void MakeNoise(Vector3 position, float loudness)
        {
            OnMakeNoise?.Invoke(position, loudness);
        }
    }
}