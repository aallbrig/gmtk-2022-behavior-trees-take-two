using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public abstract class Behavior: IBehavior
    {
        protected List<IBehavior> children = new List<IBehavior>();

        protected Behavior() => CurrentStatus = Status.Clean;
        // Need to know about parent?
        public Status CurrentStatus { get; protected set; }

        public virtual void Reset()
        {
            CurrentStatus = Status.Clean;
        }

        public abstract Status Tick();

        public IEnumerable<IBehavior> Children => children;
    }
}