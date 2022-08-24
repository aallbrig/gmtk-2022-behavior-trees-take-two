using System;

namespace Model.AI.BehaviorTrees
{
    public interface IBehaviorTreeProvider<T>
    {
        public IBehaviorTree ProvideBehaviorTree(T context);
    }
}