
using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class TaskActionContext: ITaskActionContext
    {
        private readonly Func<Status> _taskActionFunction;

        public TaskActionContext(Func<Status> taskActionFunction) => _taskActionFunction = taskActionFunction;

        public Status TaskAction() => _taskActionFunction();
    }
}