using System;

namespace Model.AI.BehaviorTrees
{
    public interface IBehavior
    {
        public Status CurrentStatus { get; }
        public Status Tick();
    }
}