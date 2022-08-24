using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using MonoBehaviours;

namespace ScriptableObjects.Agent
{
    public class GruntBehaviorTreeProvider: IBehaviorTreeProvider<Grunt>
    {
        public IBehaviorTree ProvideBehaviorTree(Grunt grunt)
        {
            var hasTarget = new Conditional(new ConditionalContext(grunt.HasTarget));
            var moveToTarget = new TaskAction(new TaskActionContext(grunt.MoveToTarget));
            // c: have target -> move to target

            var seq = new Sequence();
            seq.AddChild(hasTarget);
            seq.AddChild(moveToTarget);

            var bt = new BehaviorTree(seq);
            return bt;
        }
    }
}