using System;
using Model;
using Model.Interfaces;
using UnityEngine;

namespace MonoBehaviours
{
    public class Grunt : MonoBehaviour, ITrackTargets
    {

        public event Action<Target> TargetAcquired;
        public event Action<Target> TargetLost;
        private Transform _target;

        public void SetTarget(Target target)
        {
            _target = target.transform ? target.transform : throw new ArgumentNullException(nameof(target));
            TargetAcquired?.Invoke(target);
        }
    }
}