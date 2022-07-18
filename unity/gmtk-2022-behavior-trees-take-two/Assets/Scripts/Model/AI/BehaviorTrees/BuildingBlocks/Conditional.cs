using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class Conditional: Behavior
    {
        private readonly IConditionalContext _context;
        public Conditional(IConditionalContext conditionalContext) => _context = conditionalContext;

        public override Status Tick()
        {
            return _context.ConditionalFunction() ? Status.Success : Status.Failure;
        }
    }
}