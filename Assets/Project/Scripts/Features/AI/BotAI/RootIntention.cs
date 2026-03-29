using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AI
{
    public class RootIntention : Intention
    {
        [SerializeField] private List<Intention> _intentions;

        private Intention _activeIntention = null;

        public Intention ActiveIntention => _activeIntention;

        public void SetActive(bool active) => enabled = active;

        public void Initialize()
        {
            _intentions = _intentions.OrderByDescending(x => x.Priority).ToList();

            foreach (var intention in _intentions)
                intention.Initialize();
        }

        protected override async UniTask RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Intention next = _intentions
               .Where(x => x.isActiveAndEnabled
                           && x.Priority > (_activeIntention?.Priority ?? -1)
                           && x.IsNeedToBeActivated())
               .OrderByDescending(x => x.Priority)
               .FirstOrDefault();

                if (next != null)
                    ActivateIntention(next).Forget();

                await UniTask.NextFrame(token);
            }
        }

        public async UniTask ActivateIntention(Intention intention)
        {
            if (_activeIntention == intention) return;

            if (_activeIntention != null)
                _activeIntention.Stop();

            await UniTask.NextFrame();

            _activeIntention = intention;
            _activeIntention.OnStop += OnActiveIntentionStopped;
            _activeIntention.Activate();
        }

        private void OnActiveIntentionStopped(Intention intention)
        {
            if (_activeIntention == null) return;
            _activeIntention.OnStop -= OnActiveIntentionStopped;
            if (_activeIntention == intention)
                _activeIntention = null;
        }

        protected override void OnDisable()
        {
            // base.OnDisable();
            // _activeIntention?.Stop();
        }

        public override bool IsNeedToBeActivated() => true;

        public override void Stop()
        {
            base.Stop();
            foreach (var intention in _intentions)
            {
                intention.Stop();
            }
        }
    }
}