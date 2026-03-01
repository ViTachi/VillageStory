using Gameplay.AI;
using UnityEngine;

namespace Gameplay
{
    public class BotUnit : Unit, ICanHide
    {
        [SerializeField] private IntentionsPlayer _intentionsPlayer;

        public override Vector3 Position => transform.position;

        public override Vector3 HeadPosition => transform.position;

        public override Bounds Bounds => new Bounds(transform.position, Vector3.zero);

        public override float Radius => 0;

        public override Vector3 ForwardDirection => transform.forward;

        public bool IsHiden => false;

        private void Awake()
        {
            _intentionsPlayer.Initialize();
            _intentionsPlayer.SetActive(true);
        }
    }
}