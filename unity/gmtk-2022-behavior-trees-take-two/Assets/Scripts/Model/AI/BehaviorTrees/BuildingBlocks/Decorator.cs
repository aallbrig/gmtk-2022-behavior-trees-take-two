using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{

    public class Decorator: Composite, IDecorator
    {
        public event Action<IBehavior> ChildSet;

        private readonly IDecoratorContext _ctx;

        public Decorator(IDecoratorContext ctx, IBehavior behavior)
        {
            _ctx = ctx;
            SetOnlyChild(behavior);
        }

        public void SetOnlyChild(IBehavior behavior)
        {
            children[0] = behavior;
            ChildSet?.Invoke(behavior);
        }

        public override Status Tick()
        {
            return _ctx.RunOperation(children[0]);
        }
    }
}