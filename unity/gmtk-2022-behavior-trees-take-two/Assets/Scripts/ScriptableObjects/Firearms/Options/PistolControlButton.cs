using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Firearms.Options
{
    [CreateAssetMenu(fileName = "Pistol", menuName = "Game/UI/Firearm Control Button", order = 0)]
    public class PistolControlButton : AbstractControlButton
    {
        public override string Label => "Pistol";

        public override string Description => "Pistol Control Button";
    }
}