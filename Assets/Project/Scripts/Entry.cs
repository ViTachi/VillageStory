using GameFlow;
using UnityEngine;
using Zenject;

public class Entry : MonoBehaviour
{
    [Inject] private GameFlowStateMachine _gameFlowStateMachine;

    private void Start()
    {
        if (SceneNames.Entry.ToSceneName() == gameObject.scene.name)
            _gameFlowStateMachine.Initialize();
    }
}
