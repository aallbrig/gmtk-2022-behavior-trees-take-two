using UnityEngine;

namespace Model
{
    public class Target
    {
        public Transform Transform { get; }

        public Target(Transform targetTransform)
        {
            Transform = targetTransform;
        }
    }
}