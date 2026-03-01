using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.AI
{
    public class UnitActionsLauncher : MonoBehaviour
    {
        [SerializeField] private List<UnitActionBase> _actions;

        private readonly Dictionary<UnitActionBase, CancellationTokenSource> _runningActions
            = new Dictionary<UnitActionBase, CancellationTokenSource>();

        public async UniTask StartAction<T, TParams>(TParams parameters = default)
            where T : UnitAction<TParams>
        {
            var action = _actions.OfType<T>().FirstOrDefault();
            if (action == null)
                return;

            StopAction<T>();

            var cts = new CancellationTokenSource();
            _runningActions[action] = cts;

            try
            {
                await action.StartAction(cts.Token, parameters);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                cts.Dispose();
                _runningActions.Remove(action);
            }
        }

        public void StopAction<T>() where T : UnitActionBase
        {
            var action = _actions.OfType<T>().FirstOrDefault();
            if (action == null)
                return;

            if (_runningActions.TryGetValue(action, out var cts))
            {
                cts.Cancel();
            }
        }

        public void StopAllActions()
        {
            foreach (var kvp in _runningActions.Values)
            {
                kvp.Cancel();
            }
        }
    }
}