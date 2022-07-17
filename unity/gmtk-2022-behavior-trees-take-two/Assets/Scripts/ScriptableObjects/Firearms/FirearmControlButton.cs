using System.Collections.Generic;
using MonoBehaviours.UI;
using ScriptableObjects.Firearms.Options;
using UnityEngine;

namespace ScriptableObjects.Firearms
{
    [CreateAssetMenu(fileName = "Firearm", menuName = "Game/UI/Firearm Control Button", order = 0)]
    public class FirearmControlButton : AbstractControlButton
    {
        public List<FirearmSecondaryControlButton> firearmOptions = new List<FirearmSecondaryControlButton>();

        public override string Label => "Firearm";

        public override string Description => "Firearm Control Button";

        public override void Execute(IControlsMenu ctx)
        {
            if (firearmOptions.Count == 0) return;
        }
    }
}