using System;

namespace Model.AI.BehaviorTrees
{
    public interface IBehavior
    {
        public void OnStart();
        public void OnTerminate();
        public Status Tick();
    }
}