using System;
using Model.AI.BehaviorTrees;
using Model.Interfaces;
using ScriptableObjects.Agent;
using Unity.Plastic.Antlr3.Runtime.Debug;
using UnityEngine;

namespace MonoBehaviours.Brains
{
    public class BehaviorTreeRunner : MonoBehaviour, IBehaviorTreeRunner, IMonobehaviourDebugLogger
    {
        public BehaviorTreeRunnerConfiguration config;
        [SerializeField] private bool debugEnabled;
        [SerializeField] private float timeLastRun;
        [SerializeField] private float timeSinceLastRun;

        private void Start()
        {
            Config ??= config;
            if (Config == null)
                throw new ArgumentNullException(nameof(Config), "Behavior Tree runner needs a valid runner configuration");

            DebugLog($"Config: {Config}");
            // Behavior tree is allowed to be null
            // if (BehaviorTree == null)
                // throw new ArgumentNullException(nameof(BehaviorTree), "Behavior tree is required for a behavior tree runner to work!");

            // Behavior tree is able to be evaluated immediately
            timeLastRun = Time.time - Config.TimeBetween;
            DebugLog($"Time last run initial value: {timeLastRun}");
        }

        private void Update()
        {
            Run(Time.time);
        }

        public void Run(float time)
        {
            if (BehaviorTree == null) return;

            timeSinceLastRun = time - timeLastRun;
            if (timeSinceLastRun >= Config.TimeBetween)
            {
                timeLastRun = time;
                BehaviorTree.Step();
            }
        }

        public void SetBehaviorTree(IBehaviorTree bt)
        {
            BehaviorTree = bt;
            BehaviorTree.StepCompleted += () =>
            {
                DebugLog($"behavior tree step complete -- behavior count in bt queue {BehaviorTree.BehaviorQueue.Count}");
            };
            BehaviorTree.BehaviorTraverseCompleted += () =>
            {
                // if (debugEnabled) Debug.Log($"{name} | tree traverse complete -- resetting bt");
                DebugLog($"tree traverse complete -- resetting bt");
                BehaviorTree.Reset();
            };
        }

        public IBehaviorTreeRunnerConfiguration Config { get; private set; }
        public IBehaviorTree BehaviorTree { get; private set; }

        public void DebugLog(string logMessage)
        {
            if (debugEnabled) Debug.Log($"{name} | {logMessage}");
        }
    }

    public interface IBehaviorTreeRunner
    {
        public IBehaviorTreeRunnerConfiguration Config { get; }
        public IBehaviorTree BehaviorTree { get; }
    }
}