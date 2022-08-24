using System;
using Model.AI.BehaviorTrees;
using ScriptableObjects.Agent;
using UnityEngine;

namespace MonoBehaviours.Brains
{
    public class BehaviorTreeRunner : MonoBehaviour, IBehaviorTreeRunner
    {
        public BehaviorTreeRunnerConfiguration config;
        [SerializeField] private float timeLastRun;

        private void Start()
        {
            Config ??= config;
            if (Config == null)
                throw new ArgumentNullException(nameof(Config), "Behavior Tree runner needs a valid runner configuration");

            // Behavior tree is allowed to be null
            // if (BehaviorTree == null)
                // throw new ArgumentNullException(nameof(BehaviorTree), "Behavior tree is required for a behavior tree runner to work!");

            // Behavior tree is able to be evaluated immediately
            timeLastRun = Time.time - Config.TimeBetween;
        }

        private void Update()
        {
            Run(Time.time);
        }

        public void Run(float time)
        {
            if (BehaviorTree == null) return;

            if (time - timeLastRun >= Config.TimeBetween)
            {
                timeLastRun = time;
                BehaviorTree.Evaluate();
            }
        }

        public void SetBehaviorTree(IBehaviorTree bt) => BehaviorTree = bt;

        public IBehaviorTreeRunnerConfiguration Config { get; private set; }
        public IBehaviorTree BehaviorTree { get; private set; }
    }

    public interface IBehaviorTreeRunner
    {
        public IBehaviorTreeRunnerConfiguration Config { get; }
        public IBehaviorTree BehaviorTree { get; }
    }
}