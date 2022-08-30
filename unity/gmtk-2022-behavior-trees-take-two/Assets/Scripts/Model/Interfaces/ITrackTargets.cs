using System;
namespace Model.Interfaces
{
    public interface ITrackTargets
    {
        public event Action<Target> TargetAcquired;
        public event Action TargetLost;
        public void SetTarget(Target newTarget);
    }
}