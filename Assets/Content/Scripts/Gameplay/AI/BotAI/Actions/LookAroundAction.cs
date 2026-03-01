using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.AI
{
    public class LookAroundAction : UnitAction<LookAroundAction.Parameters>
    {
        [SerializeField] private float _durationByDefault = 3f;
        [Inject] private UnitSenses _unitSenses;

        public class Parameters
        {
            public float Duration { get; set; }
        }

        protected async override UniTask RunAction(CancellationToken token)
        {
            _unitSenses.Movement?.Stop();
            await UniTask.WaitForSeconds(_parameters == null ? _durationByDefault :
                _parameters.Duration, cancellationToken: token);
        }
    }
}