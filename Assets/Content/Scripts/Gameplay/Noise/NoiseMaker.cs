using UnityEngine;
using Zenject;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] private float loudness;

    [Inject] private NoiseHandler noiseHandler;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > 2 && loudness > 0f)
            noiseHandler.MakeNoise(transform.position, loudness);
    }
}
