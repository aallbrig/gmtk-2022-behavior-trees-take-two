using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public class BehaviorTree
    {
        public Status CurrentStatus = Status.Clean;
        public BehaviorTree(IBehavior rootNode) => _rootNode = rootNode;
        private readonly IBehavior _rootNode;
        private List<Behavior> _nodes = new List<Behavior>();
        private Dictionary<Behavior, List<Behavior>> _adjacencyLists = new Dictionary<Behavior, List<Behavior>>();

        public void Run()
        {
            TreeTraversalStarted?.Invoke();
            CurrentStatus = _rootNode.Tick();
        }

        public event Action TreeTraversalStarted;
    }
}