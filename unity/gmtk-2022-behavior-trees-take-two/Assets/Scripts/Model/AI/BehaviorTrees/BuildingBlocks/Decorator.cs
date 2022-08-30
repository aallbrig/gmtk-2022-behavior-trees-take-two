using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{

    public class Decorator: Composite, IDecorator
    {
        public event Action<IBehavior> SingleChildBehaviorSet;

        public Decorator(IBehavior behavior)
        {
            SetOnlyChild(behavior);
        }

        public void SetOnlyChild(IBehavior behavior)
        {
            if (children.Count == 0) children.Add(behavior);
            else children[0] = behavior;

            SingleChildBehaviorSet?.Invoke(behavior);
        }

        public override Status Tick(IBehaviorTree bt)
        {
            var child = children[0];
            child.Succeeded += () =>
            {
                CurrentStatus = Status.Success;
                BroadcastEventForStatus(CurrentStatus);
            };
            child.Failed += () =>
            {
                CurrentStatus = Status.Failure;
                BroadcastEventForStatus(CurrentStatus);
            };
            child.IsRunning += () =>
            {
                CurrentStatus = Status.Running;
                BroadcastEventForStatus(CurrentStatus);
            };

            if (!bt.BehaviorQueue.Contains(child)) bt.BehaviorQueue.Enqueue(child);

            return CurrentStatus;
        }
    }
}