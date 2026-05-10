using UnityEngine;
using Zenject;

namespace Noise
{
    public class NoiseMaker : MonoBehaviour
    {
        [SerializeField] private float _loudness;

        [Inject] private INoiseHandler _noiseHandler;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude > 2 && _loudness > 0f)
                _noiseHandler.MakeNoise(transform.position, _loudness);
        }
    }
}
