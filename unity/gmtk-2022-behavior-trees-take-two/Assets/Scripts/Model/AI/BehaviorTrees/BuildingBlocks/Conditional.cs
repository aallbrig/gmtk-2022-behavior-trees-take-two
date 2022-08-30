using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class Conditional: Behavior
    {
        private readonly Func<bool> _predicate;
        public Conditional(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public override Status Tick(IBehaviorTree bt)
        {
            CurrentStatus = _predicate.Invoke() ? Status.Success : Status.Failure;
            BroadcastEventForStatus(CurrentStatus);
            return CurrentStatus;
        }
    }
}