using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class TaskAction: Behavior
    {
        private readonly Func<Status> _taskActionFunction;
        public TaskAction(Func<Status> taskActionFunction)
        {
            _taskActionFunction = taskActionFunction;
        }

        public override Status Tick(IBehaviorTree bt)
        {
            CurrentStatus = _taskActionFunction.Invoke();
            BroadcastEventForStatus(CurrentStatus);
            return CurrentStatus;
        }
    }
}