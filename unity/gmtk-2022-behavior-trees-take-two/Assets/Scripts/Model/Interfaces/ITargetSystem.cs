using System;
using System.Collections.Generic;

namespace Model.Interfaces
{
    [Serializable]
    public struct TargetAcquired
    {
        public IGameAgent NewTrackedTarget;
        public IEnumerable<IGameAgent> CurrentTrackedTargets;

        public TargetAcquired(IGameAgent newTrackedTarget, IEnumerable<IGameAgent> currentTrackedTargets)
        {
            NewTrackedTarget = newTrackedTarget;
            CurrentTrackedTargets = currentTrackedTargets;
        }
    }

    [Serializable]
    public struct TrackedTargetLost
    {
        public IGameAgent LostTrackedTarget;
        public IEnumerable<IGameAgent> CurrentTrackedTargets;

        public TrackedTargetLost(IGameAgent lostTrackedTarget, IEnumerable<IGameAgent> currentTrackedTargets)
        {
            LostTrackedTarget = lostTrackedTarget;
            CurrentTrackedTargets = currentTrackedTargets;
        }
    }

    public interface ITargetSystem
    {
        public event Action<TargetAcquired> TargetAcquired;
        public event Action<TrackedTargetLost> TargetLost;

        public ITargetingSystemConfiguration Configuration { get; }

        public IEnumerable<IGameAgent> Friendlies { get; }
        public IEnumerable<IGameAgent> Enemies { get; }
        public IEnumerable<IGameAgent> Neutrals { get; }
    }
}