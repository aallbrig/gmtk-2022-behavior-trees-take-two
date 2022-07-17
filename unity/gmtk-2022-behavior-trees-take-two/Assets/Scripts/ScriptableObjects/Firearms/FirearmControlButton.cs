using UnityEngine;

namespace ScriptableObjects.Firearms
{
    [CreateAssetMenu(fileName = "Firearm", menuName = "Game/UI/Firearm Control Button", order = 0)]
    public class FirearmControlButton : AbstractControlButton
    {

        public override string Label => "Firearm";

        public override string Description => "Firearm Control Button";
    }
}