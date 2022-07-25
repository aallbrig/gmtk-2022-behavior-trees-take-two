using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public interface IDecorator
    {
        public event Action<IBehavior> ChildSet;

        public void SetOnlyChild(IBehavior behavior);
    }
}