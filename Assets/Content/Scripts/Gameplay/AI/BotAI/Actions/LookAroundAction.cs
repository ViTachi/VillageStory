using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.AI
{
    public class LookAroundAction : UnitAction<LookAroundAction.Parameters>
    {
        [SerializeField] private float _durationByDefault = 3f;

        public class Parameters
        {
            public float Duration { get; set; }
        }

        protected async override UniTask RunAction(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_parameters == null ? _durationByDefault :
                _parameters.Duration, cancellationToken: token);
        }
    }
}