using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class DecoratorContext:IDecoratorContext
    {
        private readonly Func<IBehavior, Status> _operation;

        public DecoratorContext(Func<IBehavior, Status> operation) => _operation = operation;
        public Status RunOperation(IBehavior childBehavior) => _operation.Invoke(childBehavior);
    }
}