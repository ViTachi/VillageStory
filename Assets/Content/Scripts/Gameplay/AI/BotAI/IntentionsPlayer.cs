using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Gameplay.AI
{
    public class IntentionsPlayer : MonoBehaviour
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

        private void Update()
        {
            Intention next = _intentions
                .Where(x => x.isActiveAndEnabled
                            && x.Priority > (_activeIntention?.Priority ?? -1)
                            && x.IsNeedToBeActivated())
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();

            if (next != null)
                ActivateIntention(next).Forget();
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
            _activeIntention.OnStop -= OnActiveIntentionStopped;
            if (_activeIntention == intention)
                _activeIntention = null;
        }

        private void OnDisable()
        {
            if (_activeIntention != null)
                _activeIntention.Stop();
        }
    }
}