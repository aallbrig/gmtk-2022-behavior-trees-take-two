using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using MonoBehaviours;
using UnityEngine;

namespace ScriptableObjects.Agent
{
    public class GruntBehaviorTreeProvider: IBehaviorTreeProvider<Grunt>
    {
        public IBehaviorTree ProvideBehaviorTree(Grunt grunt)
        {
            // c: have target -> ta: move to target
            var hasTarget = new Conditional(grunt.HasTarget);
            var moveToTarget = new TaskAction(grunt.MoveToTarget);

            var seq = new Sequence();
            seq.AddChild(hasTarget);
            seq.AddChild(moveToTarget);

            var bt = new BehaviorTree(seq);
            return bt;
        }
    }
}