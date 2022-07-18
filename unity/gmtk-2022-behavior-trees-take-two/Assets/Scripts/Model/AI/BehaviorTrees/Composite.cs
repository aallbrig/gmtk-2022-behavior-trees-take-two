using System;
using System.Collections.Generic;

namespace Model.AI.BehaviorTrees
{
    public abstract class Composite : Behavior
    {
        public event Action<Behavior> ChildAdded;

        protected List<Behavior> Children => _children;
        private readonly List<Behavior> _children = new List<Behavior>();

        public virtual void AddChild(Behavior child)
        {
            _children.Add(child);
            ChildAdded?.Invoke(child);
        }
    }
}