using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{

    public class Decorator: Composite, IDecorator
    {
        public event Action<IBehavior> ChildSet;

        private readonly IDecoratorContext _ctx;
        private IBehavior _child;

        public Decorator(IDecoratorContext ctx, IBehavior behavior)
        {
            _ctx = ctx;
            SetOnlyChild(behavior);
        }

        public void SetOnlyChild(IBehavior behavior)
        {
            _child = behavior;
            ChildSet?.Invoke(behavior);
        }

        public override Status Tick()
        {
            return _ctx.RunOperation(_child);
        }
    }
}