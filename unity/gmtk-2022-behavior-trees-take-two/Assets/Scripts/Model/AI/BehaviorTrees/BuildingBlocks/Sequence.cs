namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class Sequence: Composite
    {
        public override Status Tick(IBehaviorTree bt)
        {
            // What happens when sequence has no children?
            if (children.Count == 0)
            {
                CurrentStatus = Status.Failure;
                BroadcastEventForStatus(CurrentStatus);
                return CurrentStatus;
            }
            if (CurrentStatus == Status.Failure)
            {
                BroadcastEventForStatus(CurrentStatus);
                return CurrentStatus;
            }
            if (CurrentIndex >= children.Count)
            {
                CurrentIndex = 0;
                CurrentStatus = Status.Success;
                BroadcastEventForStatus(CurrentStatus);
                return CurrentStatus;
            }

            CurrentBehavior = children[CurrentIndex];
            CurrentBehavior.Succeeded += OnSucceed;
            CurrentBehavior.IsRunning += OnIsRunning;
            CurrentBehavior.Failed += OnFailed;

            bt.BehaviorQueue.Enqueue(CurrentBehavior);
            CurrentStatus = Status.Running;
            BroadcastEventForStatus(CurrentStatus);
            return CurrentStatus;
        }
        private void OnSucceed()
        {
            CurrentIndex++;
            CurrentStatus = CurrentIndex >= children.Count ? Status.Success : Status.Running;
            BroadcastEventForStatus(CurrentStatus);
            CurrentBehavior.Succeeded -= OnSucceed;
            CurrentBehavior.IsRunning -= OnIsRunning;
        }
        private void OnFailed()
        {
            CurrentStatus = Status.Failure;
            BroadcastEventForStatus(CurrentStatus);
            CurrentBehavior.Failed -= OnFailed;
            CurrentBehavior.IsRunning -= OnIsRunning;
        }
        private void OnIsRunning()
        {
            CurrentStatus = Status.Running;
            BroadcastEventForStatus(CurrentStatus);
        }
        private IBehavior CurrentBehavior { get; set; }
    }
}