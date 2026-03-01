using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.AI
{
    public class BaseHearing : Hearing
    {
        [Inject] private INoiseHandler _noiseHandler;
        [Inject] private IPathFinder _pathFinder;
        private NavMeshPath path;

        private void Awake()
        {
            path = new NavMeshPath();
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
            path = _pathFinder.GetPath(transform.position, position, out float distance);
            if (path.status == NavMeshPathStatus.PathComplete
                && distance <= _hearingDistance)
            {
                HearNoise(position);
            }
        }
    }
}