using System;
using UnityEngine;

namespace Noise
{
    public interface INoiseHandler
    {
        event Action<Vector3, float> OnMakeNoise;
        void MakeNoise(Vector3 position, float loudness);
    }
}