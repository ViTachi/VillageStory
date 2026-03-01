using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.AI
{
    public class WanderingIntention : Intention
    {
        [SerializeField] private int index;
        [Inject] private Location _location;

        public override bool IsNeedToBeActivated() => true;

        protected override async UniTask RunAsync(CancellationToken token)
        {
            int targetIndex = 0;

            while (!token.IsCancellationRequested)
            {
                var moveParams = new MoveAction.Parameters
                {
                    TargetPosition = _location.Points(index)[targetIndex].position
                };

                await _unitActionsLauncher.StartAction<MoveAction, MoveAction.Parameters>(moveParams);
                await _unitActionsLauncher.StartAction<LookAroundAction, LookAroundAction.Parameters>(null);

                targetIndex = (targetIndex + 1) % _location.Points(index).Count;
            }
        }

        public override void Stop()
        {
            base.Stop();
            _unitActionsLauncher.StopAction<LookAroundAction>();
            _unitActionsLauncher.StopAction<MoveAction>();
        }
    }
}