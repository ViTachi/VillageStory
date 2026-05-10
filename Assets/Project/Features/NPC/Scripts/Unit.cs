using System;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public virtual Vector3 Position { get; }
        public virtual Vector3 HeadPosition { get; }

        public virtual Bounds Bounds { get; }
        public virtual float Radius { get; }
        public virtual Vector3 ForwardDirection { get; }
    }
}
