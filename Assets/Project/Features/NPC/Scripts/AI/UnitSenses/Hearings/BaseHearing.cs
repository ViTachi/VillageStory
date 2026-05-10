using Noise;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace AI
{
    public class BaseHearing : Hearing
    {
        [Inject] private INoiseHandler _noiseHandler;
        [Inject] private readonly IPathFindService _pathFinder;
        private NavMeshPath _path;

        private void Awake()
        {
            _path = new NavMeshPath();
        }

        private void OnEnable()
        {
            _noiseHandler.OnMakeNoise += OnNoise;
        }

        private void OnDisable()
        {
            _noiseHandler.OnMakeNoise -= OnNoise;
        }

        private void OnNoise(Vector3 position, float loudness)
        {
            _path = _pathFinder.GetPath(transform.position, position, out float distance);
            if (_path.status == NavMeshPathStatus.PathComplete && distance <= _hearingDistance)
            {
                HearNoise(position);
            }
        }
    }
}
