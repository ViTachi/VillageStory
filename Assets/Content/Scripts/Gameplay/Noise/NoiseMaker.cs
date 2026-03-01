using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class NoiseMaker : MonoBehaviour
    {
        [SerializeField] private float loudness;

        [Inject] private INoiseHandler noiseHandler;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude > 2 && loudness > 0f)
                noiseHandler.MakeNoise(transform.position, loudness);
        }
    }
}
