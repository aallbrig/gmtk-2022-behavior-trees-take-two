using System;
using UnityEngine;

namespace Model.Interfaces
{
    public interface ITrackTargets
    {
        public event Action<Target> TargetAcquired;
        public event Action<Target> TargetLost;
        public void SetTarget(Target target);
    }
}