using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public interface IBehavior
    {
        public Status CurrentStatus { get; }
        public Status Tick();
        public IEnumerable<IBehavior> Children { get; }
    }
}