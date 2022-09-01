using Model.AI.BehaviorTrees;
using UnityEngine;

namespace ScriptableObjects.Agent
{
    public abstract class BehaviorTreeProvider : ScriptableObject
    {
        public abstract IBehaviorTree Provide();
    }
}