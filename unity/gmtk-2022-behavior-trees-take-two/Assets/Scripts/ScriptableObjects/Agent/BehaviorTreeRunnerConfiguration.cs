using UnityEngine;

namespace ScriptableObjects.Agent
{
    [CreateAssetMenu(fileName = "behavior tree runner config", menuName = "Game/new behavior tree runner config", order = 0)]
    public class BehaviorTreeRunnerConfiguration : ScriptableObject, IBehaviorTreeRunnerConfiguration
    {
        public float timeBetween = 0.25f;
        public TimeDelayType delayType = TimeDelayType.Step;

        public float TimeBetween => timeBetween;

        public TimeDelayType DelayType => delayType;
    }

    public interface IBehaviorTreeRunnerConfiguration
    {
        public float TimeBetween { get; }
        public TimeDelayType DelayType { get; }
    }

    public enum TimeDelayType
    {
        Step, FullTraverse
    }
}