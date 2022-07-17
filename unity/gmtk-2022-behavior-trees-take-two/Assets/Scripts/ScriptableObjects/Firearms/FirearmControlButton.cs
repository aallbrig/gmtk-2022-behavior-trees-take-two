using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Firearms
{
    [CreateAssetMenu(fileName = "Firearm", menuName = "Game/UI/Firearm Control Button", order = 0)]
    public class FirearmControlButton : AbstractControlButton
    {
        public List<AbstractFirearm> firearmOptions = new List<AbstractFirearm>();

        public override string Label => "Firearm";

        public override string Description => "Firearm Control Button";
    }
}