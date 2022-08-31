namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class Sequence: Composite
    {
        public override Status Tick(IBehaviorTree bt)
        {
            // What happens when sequence has no children?
            if (children.Count == 0) return Status.Failure;
            if (CurrentStatus == Status.Failure) return CurrentStatus;
            if (CurrentIndex >= children.Count)
            {
                CurrentIndex = 0;
                return Status.Success;
            }

            var b = children[CurrentIndex];
            b.Succeeded += () =>
            {
                CurrentIndex++;
                CurrentStatus = CurrentIndex >= children.Count ? Status.Success : Status.Running;
                BroadcastEventForStatus(CurrentStatus);
            };
            b.IsRunning += () =>
            {
                CurrentStatus = Status.Running;
                BroadcastEventForStatus(CurrentStatus);
            };
            b.Failed += () =>
            {
                CurrentStatus = Status.Failure;
                BroadcastEventForStatus(CurrentStatus);
            };

            bt.BehaviorQueue.Enqueue(b);
            return Status.Running;
            // foreach (var currentBehavior in Children)
            // {
            //     switch (currentBehavior.Tick(bt))
            //     {
            //         case Status.Success:
            //             CurrentStatus = Status.Running;
            //             break; // Iterate to the next behavior
            //         case Status.Running:
            //             CurrentStatus = Status.Running;
            //             return CurrentStatus;
            //         case Status.Failure:
            //             CurrentStatus = Status.Failure;
            //             return CurrentStatus;
            //     }
            // }
            // return Status.Success;
        }
    }
}