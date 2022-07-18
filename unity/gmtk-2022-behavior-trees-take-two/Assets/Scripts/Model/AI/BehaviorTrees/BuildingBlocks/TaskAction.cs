namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class TaskAction: Behavior
    {
        private readonly ITaskActionContext _taskActionContext;
        public TaskAction(ITaskActionContext taskActionContext) => _taskActionContext = taskActionContext;

        public override Status Tick() => _taskActionContext.TaskAction();
    }
}