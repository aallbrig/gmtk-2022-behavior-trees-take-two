using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public interface IDecoratorContext
    {
        public Status RunOperation(IBehavior childBehavior);
    }
}