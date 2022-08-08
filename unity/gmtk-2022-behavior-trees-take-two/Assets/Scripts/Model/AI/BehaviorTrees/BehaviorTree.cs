using System;
using System.Collections.Generic;
using Model.AI.BehaviorTrees.BuildingBlocks;

namespace Model.AI.BehaviorTrees
{
    public class BehaviorTree
    {
        public event Action TreeTraversalCompleted;
        public Status CurrentStatus = Status.Clean;
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
            _rootNode = new Decorator(
                new DecoratorContext(childBehavior => childBehavior.Tick()),
                firstChildBehavior
            );
            BuildTreeGraph();
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

        public void Run()
        {
            SimpleImplicitTreeTraversal();
            TreeTraversalCompleted?.Invoke();
        }

        private void SimpleImplicitTreeTraversal()
        {
            var status = _rootNode.Tick();
            CurrentStatus = status;
        }
    }
}