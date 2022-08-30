using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public abstract class Composite : Behavior
    {
        public event Action<IBehavior> ChildAdded;
        protected int CurrentIndex = 0;

        public virtual void AddChild(IBehavior child)
        {
            children.Add(child);
            ChildAdded?.Invoke(child);
        }

        public override void Reset()
        {
            base.Reset();
            CurrentIndex = 0;
        }
    }
}