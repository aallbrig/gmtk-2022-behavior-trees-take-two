using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Firearms.Options
{
    [CreateAssetMenu(fileName = "Firearm secondary control button", menuName = "Game/UI/Firearm Secondary Control Button", order = 0)]
    public class FirearmSecondaryControlButton : AbstractControlButton
    {
        public AbstractFirearm firearm;
        public override string Label => firearm.Name;

        public override string Description => $"{firearm.Name} Control Button";
    }
}