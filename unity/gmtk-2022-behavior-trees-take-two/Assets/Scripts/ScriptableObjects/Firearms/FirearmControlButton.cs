using System.Collections.Generic;
using System.Linq;
using MonoBehaviours.UI;
using UnityEngine;

namespace ScriptableObjects.Firearms
{
    [CreateAssetMenu(fileName = "Firearm", menuName = "Game/UI/Firearm Control Button", order = 0)]
    public class FirearmControlButton : AbstractControlButton
    {
        public List<Firearm> firearmOptions = new List<Firearm>();

        public override string Label => "Firearm";

        public override string Description => "Firearm Control Button";

        public override void Execute(IControlsMenu ctx)
        {
            if (firearmOptions.Count == 0) return;

            ctx.PopulateSecondaryMenu(firearmOptions.ToList<IControlButton>());
        }
    }
}