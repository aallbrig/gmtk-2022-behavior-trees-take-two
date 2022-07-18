namespace Model.AI.BehaviorTrees
{
    public abstract class Behavior: IBehavior
    {
        protected Behavior() => CurrentStatus = Status.Clean;
        // Need to know about parent?
        public Status CurrentStatus { get; protected set; }

        public virtual void Reset()
        {
            CurrentStatus = Status.Clean;
        }

        public abstract Status Tick();
    }
}