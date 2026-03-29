using System.Threading;
using Cysharp.Threading.Tasks;

public class State
{
    protected StateMachine _stateMachine;

    public void Construct(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual UniTask Enter(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }

    public virtual UniTask Exit(CancellationToken token)
    {
        return UniTask.CompletedTask;
    }
}