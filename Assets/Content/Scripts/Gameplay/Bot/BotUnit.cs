using System;
using Gameplay.AI;
using UnityEngine;

namespace Gameplay.Units
{
    public class BotUnit : Unit, ICanHide, ICanDie
    {
        [SerializeField] private IntentionsPlayer _intentionsPlayer;

        public event Action OnDie;

        public override Vector3 Position => transform.position;
        public override Vector3 HeadPosition => transform.position;
        public override Bounds Bounds => new Bounds(transform.position, Vector3.zero);
        public override float Radius => 0;
        public override Vector3 ForwardDirection => transform.forward;
        public bool IsHiden => false;
        public bool IsAlive => _isAlive;

        private bool _isAlive = true;

        private void Awake()
        {
            _intentionsPlayer.Initialize();
            _intentionsPlayer.SetActive(true);
        }

        public void Die()
        {
            OnDie?.Invoke();
            _isAlive = false;
        }
    }
}