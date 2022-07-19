using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public class BehaviorTree
    {
        public event Action TreeTraversalStarted;
        public event Action TreeTraversalCompleted;
        public Status CurrentStatus = Status.Clean;
        public BehaviorTree(IBehavior rootNode) => _rootNode = rootNode;
        private readonly IBehavior _rootNode;
        private List<Behavior> _nodes = new List<Behavior>();
        private Dictionary<Behavior, List<Behavior>> _adjacencyLists = new Dictionary<Behavior, List<Behavior>>();
        private readonly Queue<IBehavior> _behaviorQueue = new Queue<IBehavior>();

        public void Run()
        {
            TreeTraversalStarted?.Invoke();
            TraverseTreeAlgorithm();
            TreeTraversalCompleted?.Invoke();
        }

        private void TraverseTreeAlgorithm()
        {
            if (_behaviorQueue.Count == 0) _behaviorQueue.Enqueue(_rootNode);

            while (_behaviorQueue.Count > 0)
            {
                var currentBehavior = _behaviorQueue.Dequeue();
                var currentStatus = currentBehavior.Tick();
                // switch (currentStatus)
                // {
                    // case Status.Success:
                    // break;
                    // case Status.Failure:
                        // break;
                    // case Status.Running:
                        // break;
                // }
            }
        }
    }
}