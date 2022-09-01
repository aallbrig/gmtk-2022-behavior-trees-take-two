using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using MonoBehaviours;
using UnityEngine;

namespace ScriptableObjects.Agent
{
    public class MasterChiefBehaviorTreeProvider: IBehaviorTreeProvider<MasterChief>
    {
        public IBehaviorTree ProvideBehaviorTree(MasterChief mc)
        {
            var isProcessingUserInput = new Conditional(mc.ProcessingUserInput);
            var logLocomotionIntention = new TaskAction(() =>
            {
                mc.DebugLog("locomotion intention");
                return Status.Success;
            });
            var hasTarget = new Conditional(mc.HasCurrentTarget);
            var logCombatIntention = new TaskAction(() =>
            {
                mc.ShootTarget();
                mc.DebugLog("intention to engage combat with target");
                return Status.Success;
            });
            var masterChiefLocomotionSeq = new Sequence();
            masterChiefLocomotionSeq.AddChild(isProcessingUserInput);
            masterChiefLocomotionSeq.AddChild(logLocomotionIntention);
            var masterChiefCombatSeq = new Sequence();
            masterChiefCombatSeq.AddChild(hasTarget);
            masterChiefCombatSeq.AddChild(logCombatIntention);
            var rootFallback = new Fallback();
            rootFallback.AddChild(masterChiefLocomotionSeq);
            rootFallback.AddChild(masterChiefCombatSeq);
            var bt = new BehaviorTree(rootFallback);
            return bt;
        }
    }
}