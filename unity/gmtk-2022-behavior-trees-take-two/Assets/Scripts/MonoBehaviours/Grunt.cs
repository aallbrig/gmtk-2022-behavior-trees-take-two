using System;
using Model;
using Model.AI.BehaviorTrees;
using Model.AI.BehaviorTrees.BuildingBlocks;
using Model.Interfaces;
using UnityEngine;

namespace MonoBehaviours
{
    public class Grunt : MonoBehaviour, ITrackTargets, IBehaviorTreeProvider
    {
        public event Action<Target> TargetAcquired;
        public event Action<Target> TargetLost;
        public float thinkRateInSeconds = 1f;
        private float _timeOfLastThought;
        private Transform _target;
        private BehaviorTree _brain;

        private void Start()
        {
            _brain = ProvideBehaviorTree();
            // let the brain think instantly
            _timeOfLastThought = Time.time - thinkRateInSeconds;
        }

        private void Update()
        {
            if (Time.time - _timeOfLastThought > thinkRateInSeconds) _brain.Run();
        }

        public void SetTarget(Target target)
        {
            _target = target.transform ? target.transform : throw new ArgumentNullException(nameof(target));
            TargetAcquired?.Invoke(target);
        }

        public event Action<BehaviorTree> BehaviorTreeProvided;

        public BehaviorTree ProvideBehaviorTree()
        {
            var firstChild = new NullBehavior();
            var bt = new BehaviorTree(firstChild);
            BehaviorTreeProvided?.Invoke(bt);
            return bt;
        }
    }
}