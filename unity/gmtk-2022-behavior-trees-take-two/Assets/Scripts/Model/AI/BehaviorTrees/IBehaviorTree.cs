using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public interface IBehaviorTree
    {
        public Status CurrentStatus { get; }
        public IBehavior CurrentBehavior { get; }
        public Queue<IBehavior> BehaviorQueue { get; }
        public void Reset();
        public void Evaluate();
        public void Step();
        public event Action StepCompleted;
        public event Action BehaviorTraverseCompleted;
    }
}