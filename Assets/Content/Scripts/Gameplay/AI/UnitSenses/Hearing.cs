using System;
using UnityEngine;

namespace Gameplay.AI
{
    public abstract class Hearing : MonoBehaviour
    {
        [SerializeField] protected float _hearingDistance;

        protected IntentionsPlayer _aIPlayer;

        public event Action<Vector3> OnHearNoise;

        protected virtual void HearNoise(Vector3 position)
        {
            OnHearNoise?.Invoke(position);
        }
    }
}