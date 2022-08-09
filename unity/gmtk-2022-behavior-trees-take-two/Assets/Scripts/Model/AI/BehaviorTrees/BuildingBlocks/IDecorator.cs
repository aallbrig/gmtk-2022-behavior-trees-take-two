using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public interface IDecorator
    {
        public event Action<IBehavior> SingleChildBehaviorSet;

        public void SetOnlyChild(IBehavior behavior);
    }
}