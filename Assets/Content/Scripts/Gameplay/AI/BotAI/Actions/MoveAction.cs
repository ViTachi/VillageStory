using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.AI
{
    public class MoveAction : UnitAction<MoveAction.Parameters>
    {
        [Inject] private UnitSenses _unitSenses;

        public class Parameters
        {
            public Vector3 TargetPosition { get; set; }
            public Transform Target { get; set; }
            public float StopDistance { get; set; } = 0f;
        }

        protected async override UniTask RunAction(CancellationToken token)
        {
            var movement = _unitSenses.Movement;
            var parameters = _parameters;

            if (parameters == null || movement == null)
            {
                return;
            }

            if (parameters.Target != null)
                movement.MoveToTarget(parameters.Target, parameters.StopDistance);
            else
                movement.MoveToPoint(parameters.TargetPosition, parameters.StopDistance);

            await UniTask.WaitUntil(() => movement.IsTargetReached, cancellationToken: token);
        }

        public override void StopAction()
        {
            base.StopAction();

            if (_parameters != null && _unitSenses.Movement != null)
                _unitSenses.Movement.Stop();
        }
    }
}