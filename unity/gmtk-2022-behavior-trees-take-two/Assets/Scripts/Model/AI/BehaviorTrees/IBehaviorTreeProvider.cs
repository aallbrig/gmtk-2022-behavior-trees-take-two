using System;

namespace Model.AI.BehaviorTrees
{
    public interface IBehaviorTreeProvider
    {
        public event Action<BehaviorTree> BehaviorTreeProvided;

        public BehaviorTree ProvideBehaviorTree();
    }
}