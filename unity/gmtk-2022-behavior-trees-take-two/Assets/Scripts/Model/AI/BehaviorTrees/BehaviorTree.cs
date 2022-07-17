using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public class BehaviorTree
    {
        public Status Status = Status.Failure;
        public BehaviorTree(IBehavior rootNode) => _rootNode = rootNode;
        private IBehavior _rootNode;
        public void Run()
        {
            TreeTraversalStarted?.Invoke();
            Status = _rootNode.Tick();
        }

        public event Action TreeTraversalStarted;
    }
}