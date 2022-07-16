using UnityEngine;

namespace Model
{
    public class Target
    {
        public Transform transform { get; }

        public Target(Transform targetTransform)
        {
            transform = targetTransform;
        }
    }
}