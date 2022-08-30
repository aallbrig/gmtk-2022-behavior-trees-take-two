namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    // Aka "Selector" but Fallback is its more modern name.
    public class Fallback: Composite
    {
        public override Status Tick(IBehaviorTree bt)
        {
            foreach (var currentBehavior in Children)
            {
                switch (currentBehavior.Tick(bt))
                {
                    case Status.Success:
                        CurrentStatus = Status.Success;
                        return CurrentStatus;
                    case Status.Running:
                        CurrentStatus = Status.Running;
                        return CurrentStatus;
                    case Status.Failure:
                        CurrentStatus = Status.Running;
                        break; // Iterate to the next behavior
                }
            }
            return Status.Failure;
        }
    }
}