using Model.Interfaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "new target system configuration", menuName = "Game/new TargetSystemConfiguration", order = 0)]
    public class TargetSystemConfiguration : ScriptableObject, ITargetingSystemConfiguration
    {
        public float detectRadius = 5f;

        public float DetectRadius => detectRadius;
    }
}