using System;
using System.Collections.Generic;
using Model.AI.BehaviorTrees.BuildingBlocks;

namespace Model.AI.BehaviorTrees
{
    public class BehaviorTree: IBehaviorTree
    {
        public override string ToString()
        {
            // TODO
            // Use _nodes and _adjacencyList to construct tree
            // e.g.
            // R - D - A
            //  \- S - F - A
            //     |    \- A
            //      \- C 
            // ... or something cool like this
            return base.ToString();
        }
        public BehaviorTree(IBehavior firstChildBehavior)
        {
            _rootNode = new Decorator(firstChildBehavior);
            BuildTreeGraph();
            Reset();
            BehaviorQueue.Enqueue(_rootNode);
        }

        private void BuildTreeGraph()
        {
            // traverse _rootNode's children using DFS
            // Graph = Vertexes + Edges
            // behavior == vertex == node
            // behavior children == edge from parent to child == adjacency
            _treeTraversalQueue.Enqueue(_rootNode);
            while (_treeTraversalQueue.Count > 0)
            {
                var b = _treeTraversalQueue.Dequeue();
                CurrentBehavior = b;
                if (_nodes.Contains(b) == false) _nodes.Add(b);
                if (_adjacencyLists.ContainsKey(b) == false) _adjacencyLists[b] = new List<IBehavior>();

                foreach (var child in b.Children)
                {
                    _adjacencyLists[b].Add(child);
                    _treeTraversalQueue.Enqueue(child);
                }
            }
        }

        private readonly Decorator _rootNode;
        private readonly List<IBehavior> _nodes = new List<IBehavior>();
        private readonly Dictionary<IBehavior, List<IBehavior>> _adjacencyLists = new Dictionary<IBehavior, List<IBehavior>>();
        private readonly Queue<IBehavior> _treeTraversalQueue = new Queue<IBehavior>();

        private void SimpleImplicitTreeTraversal()
        {
            var status = _rootNode.Tick(this);
            CurrentStatus = status;
            BehaviorTraverseCompleted?.Invoke();
        }

        public Status CurrentStatus { get; private set; } = Status.Clean;

        public IBehavior CurrentBehavior { get; private set; }

        public Queue<IBehavior> BehaviorQueue { get; } = new Queue<IBehavior>();

        public void Reset()
        {
            CurrentStatus = Status.Clean;
            CurrentBehavior = null;
            BehaviorQueue.Clear();
            BehaviorQueue.Enqueue(_rootNode);
            _nodes.ForEach(node => node.Reset());
        }

        public void Evaluate()
        {
            // SimpleImplicitTreeTraversal();
            EventDrivenTreeTraversal();
        }

        public void Step()
        {
            if (BehaviorQueue.Count == 0)
            {
                BehaviorTraverseCompleted?.Invoke();
                return;
            }

            var currentBehavior = BehaviorQueue.Dequeue();

            var status = currentBehavior.Tick(this);
            if (status == Status.Running)
                BehaviorQueue.Enqueue(currentBehavior);

            StepCompleted?.Invoke();
        }

        private void EventDrivenTreeTraversal()
        {
            while (BehaviorQueue.Count > 0)
                Step();
            BehaviorTraverseCompleted?.Invoke();
        }

        public event Action StepCompleted;
        public event Action BehaviorTraverseCompleted;
    }
}