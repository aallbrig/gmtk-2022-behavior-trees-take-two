using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public abstract class Composite : Behavior
    {
        public event Action<Behavior> ChildAdded;

        public virtual void AddChild(Behavior child)
        {
            children.Add(child);
            ChildAdded?.Invoke(child);
        }
    }
}