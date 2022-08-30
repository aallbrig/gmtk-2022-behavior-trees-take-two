using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public interface IBehavior
    {
        public event Action IsRunning;
        public event Action Failed;
        public event Action Succeeded;
        public Status CurrentStatus { get; }
        public Status Tick(IBehaviorTree bt);
        public void Reset();
        public IEnumerable<IBehavior> Children { get; }
    }
}