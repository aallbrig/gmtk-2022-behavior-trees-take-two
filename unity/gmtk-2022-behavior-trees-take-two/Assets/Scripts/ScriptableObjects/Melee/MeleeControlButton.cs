using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Melee
{
    [CreateAssetMenu(fileName = "Melee Control Button", menuName = "Game/UI/Melee Control Button", order = 0)]
    public class MeleeControlButton : AbstractControlButton
    {
        public List<AbstractMelee> meleeOptions = new List<AbstractMelee>();
        public override string Label => "Melee";
        public override string Description => "Melee Control Button";
    }
}