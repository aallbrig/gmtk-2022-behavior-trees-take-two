using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class NullBehavior: IBehavior
    {
        public event Action IsRunning;
        public event Action Failed;
        public event Action Succeeded;

        public Status CurrentStatus => Status.Clean;

        public Status Tick(IBehaviorTree bt) => CurrentStatus;
        public void Reset() {}

        public IEnumerable<IBehavior> Children => default;
    }
}