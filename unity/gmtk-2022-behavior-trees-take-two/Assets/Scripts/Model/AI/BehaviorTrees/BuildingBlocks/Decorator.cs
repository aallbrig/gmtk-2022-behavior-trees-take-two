using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{

    public class Decorator: Composite, IDecorator
    {
        public event Action<IBehavior> SingleChildBehaviorSet;

        private readonly IDecoratorContext _ctx;

        public Decorator(IDecoratorContext ctx, IBehavior behavior)
        {
            _ctx = ctx;
            SetOnlyChild(behavior);
        }

        public void SetOnlyChild(IBehavior behavior)
        {
            if (children.Count == 0) children.Add(behavior);
            else children[0] = behavior;

            SingleChildBehaviorSet?.Invoke(behavior);
        }

        public override Status Tick()
        {
            return _ctx.RunOperation(children[0]);
        }
    }
}