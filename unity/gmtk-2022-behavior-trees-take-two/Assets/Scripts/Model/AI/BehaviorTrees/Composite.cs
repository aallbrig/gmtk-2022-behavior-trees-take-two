using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public abstract class Composite : Behavior
    {
        public event Action<Behavior> ChildAdded;

        public List<Behavior> Children { get; } = new List<Behavior>();

        public virtual void AddChild(Behavior child)
        {
            Children.Add(child);
            ChildAdded?.Invoke(child);
        }
    }
}