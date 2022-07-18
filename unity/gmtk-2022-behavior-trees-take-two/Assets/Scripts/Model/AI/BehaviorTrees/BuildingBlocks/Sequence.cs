namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class Sequence: Composite
    {
        public override Status Tick()
        {
            foreach (var currentBehavior in Children)
            {
                switch (currentBehavior.Tick())
                {
                    case Status.Success:
                        CurrentStatus = Status.Running;
                        break; // Iterate to the next behavior
                    case Status.Running:
                        CurrentStatus = Status.Running;
                        return CurrentStatus;
                    case Status.Failure:
                        CurrentStatus = Status.Failure;
                        return CurrentStatus;
                }
            }
            return Status.Success;
        }
    }
}