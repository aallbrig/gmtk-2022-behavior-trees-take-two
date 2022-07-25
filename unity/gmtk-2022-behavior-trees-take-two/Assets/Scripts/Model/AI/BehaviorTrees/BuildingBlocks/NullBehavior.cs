namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class NullBehavior: IBehavior
    {

        public Status CurrentStatus => Status.Clean;

        public Status Tick() => CurrentStatus;
    }
}