using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Grenades
{
    [CreateAssetMenu(fileName = "Grenades Control Button", menuName = "Game/UI/Grenade Control Button", order = 0)]
    public class GrenadeControlButton : AbstractControlButton
    {
        public List<AbstractGrenade> grenadeOptions = new List<AbstractGrenade>();
        public override string Label => "Grenade";
        public override string Description => "Grenade Control Button";
    }
}