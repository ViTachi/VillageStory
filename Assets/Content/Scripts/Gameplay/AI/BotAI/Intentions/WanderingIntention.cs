using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.AI
{
    public class WanderingIntention : Intention
    {
        [SerializeField] private List<Transform> _points;

        [Inject] private UnitActionsLauncher _actionsLauncher;

        public override bool IsNeedToBeActivated() => true;

        private int targetPointIndex = 0;
        private CancellationTokenSource _cts;

        public override void Activate()
        {
            base.Activate();

            _cts = new CancellationTokenSource();
            Wandering(_cts.Token).Forget();
        }

        private async UniTask Wandering(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await _actionsLauncher.StartAction<MoveAction, MoveAction.Parameters>(
                    new MoveAction.Parameters
                    {
                        TargetPosition = _points[targetPointIndex].position
                    });

                await _actionsLauncher.StartAction<LookAroundAction, LookAroundAction.Parameters>();

                targetPointIndex++;
                if (targetPointIndex >= _points.Count)
                    targetPointIndex = 0;
            }
        }

        public override void Stop()
        {
            base.Stop();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _actionsLauncher.StopAllActions();
        }
    }
}