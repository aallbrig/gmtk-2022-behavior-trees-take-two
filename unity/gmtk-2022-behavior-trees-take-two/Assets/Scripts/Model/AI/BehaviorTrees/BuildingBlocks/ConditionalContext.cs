using System;

namespace Model.AI.BehaviorTrees.BuildingBlocks
{
    public class ConditionalContext: IConditionalContext
    {
        private Func<bool> _predicate;
        public ConditionalContext(Func<bool> predicateFunction) => _predicate = predicateFunction;
        public bool ConditionalFunction() => _predicate.Invoke();
    }
}