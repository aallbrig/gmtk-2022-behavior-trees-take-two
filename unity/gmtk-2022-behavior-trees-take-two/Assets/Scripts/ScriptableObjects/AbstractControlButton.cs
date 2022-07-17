using MonoBehaviours.UI;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class AbstractControlButton : ScriptableObject, IControlButton
    {
        public abstract string Label { get; }

        public abstract string Description { get; }

        public void Execute()
        {
            Debug.Log("Melee Control Button executed");
        }
    }
}