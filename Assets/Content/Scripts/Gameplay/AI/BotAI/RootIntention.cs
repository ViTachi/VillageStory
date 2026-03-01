using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.AI
{
    public class RootIntention : Intention
    {
        [Title("Intentions")]
        [GUIColor(0.8f, 1f, 1f, 1f)]
        [SerializeField] private List<Intention> _intentions;

        private Intention _activeIntention = null;

        public Intention ActiveIntention => _activeIntention;

        public override bool IsNeedToBeActivated() => true;

        public override void Initialize(Intention rootIntention = null)
        {
            base.Initialize(rootIntention);
            _intentions = _intentions.OrderByDescending(x => x.Priority).ToList();

            foreach (var intention in _intentions)
            {
                intention.Initialize(this);
            }
        }

        public override void Stop()
        {
            base.Stop();
            _activeIntention?.Stop();
            _activeIntention = null;
        }

        private void Update()
        {
            if (!IsRunning) return;

            Intention nextIntention = _intentions
                .Where(x => x.isActiveAndEnabled
                && x.Priority > (_activeIntention?.Priority ?? -1)
                && x.IsNeedToBeActivated())
                .OrderByDescending(x => x.Priority).FirstOrDefault();

            if (nextIntention != null)
                ActivateIntention(nextIntention);

            if (_activeIntention != null)
            {
                if (_activeIntention.IsCompleted)
                    _activeIntention = null;
                else
                    _activeIntention.Run();
            }
        }

        public void ActivateIntention(Intention intention)
        {
            _activeIntention?.Stop();
            _activeIntention = intention;
            _activeIntention.OnStop += OnActiveIntentionStopped;
            _activeIntention.Activate();
        }

        private void OnActiveIntentionStopped(Intention intention)
        {
            _activeIntention.OnStop -= OnActiveIntentionStopped;
            _activeIntention = null;
        }
    }
}