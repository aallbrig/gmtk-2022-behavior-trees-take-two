using System.Collections.Generic;
using MonoBehaviours.UI;
using UnityEngine;

namespace ScriptableObjects.Firearms.Options
{
    [CreateAssetMenu(fileName = "Firearm secondary control button", menuName = "Game/UI/Firearm Secondary Control Button", order = 0)]
    public class FirearmSecondaryControlButton : AbstractControlButton
    {
        public Firearm firearm;
        public override string Label => firearm.firearmName;

        public override string Description => $"{firearm.firearmName} Control Button";

        public override void Execute(IControlsMenu ctx)
        {
            ctx.ResetSecondaryMenu();
        }
    }
}