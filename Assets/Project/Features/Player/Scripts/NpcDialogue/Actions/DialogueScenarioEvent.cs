using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Player
{
    [Serializable]
    public class DialogueScenarioEvent : DialogueAction
    {
        [Inject] private ISaveLoadService _saveLoadService;

        [field: SerializeField] public GameScenarioEvent ScenarioEvent { get; private set; }

        public override UniTask Execute(DialogueContext context)
        {
            if (ScenarioEvent == null)
                return UniTask.CompletedTask;

            _saveLoadService.Data.GameScenario ??= new GameScenarioSaveData();
            _saveLoadService.Data.GameScenario.AddScenarioEvent(ScenarioEvent);
            _saveLoadService.Save();

            return UniTask.CompletedTask;
        }
    }
}
