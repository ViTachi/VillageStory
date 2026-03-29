using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class StateMachine : IInitializable
{
    [Inject] private DiContainer _diContainer;

    private Dictionary<Type, State> _states = new();
    private State _activeState = null;

    [Inject]
    public virtual void Initialize()
    {

    }

    protected void Add<T>() where T : State
    {
        T state = _diContainer.Instantiate<T>();
        state.Construct(this);
        _states[typeof(T)] = state;
    }

    public void EnterState<T>() where T : State
    {
        ChangeToState<T>().Forget();
    }

    private async UniTask ChangeToState<T>() where T : State
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        if (_activeState != null)
            await _activeState.Exit(cts.Token);
        if (_states.TryGetValue(typeof(T), out State state))
        {
            _activeState = state;
            await state.Enter(cts.Token);
        }
        else
        {
            Debug.LogError($"{typeof(T).Name} state is not exist in State Machine");
        }
    }
}
