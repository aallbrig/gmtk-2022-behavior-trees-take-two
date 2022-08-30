using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public abstract class Behavior: IBehavior
    {
        protected readonly List<IBehavior> children = new List<IBehavior>();

        protected Behavior() => CurrentStatus = Status.Clean;
        // Need to know about parent?
        public event Action IsRunning;
        public event Action Failed;
        public event Action Succeeded;

        protected void BroadcastEventForStatus(Status status)
        {
            switch (status)
            {
                case Status.Failure:
                    Failed?.Invoke();
                    break;
                case Status.Success:
                    Succeeded?.Invoke();
                    break;
                case Status.Running:
                    IsRunning?.Invoke();
                    break;
            }
        }

        public Status CurrentStatus { get; protected set; }

        public virtual void Reset()
        {
            CurrentStatus = Status.Clean;
        }

        public abstract Status Tick(IBehaviorTree bt);

        public IEnumerable<IBehavior> Children => children;
    }
}