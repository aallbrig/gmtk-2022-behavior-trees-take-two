using UnityEngine;

namespace ScriptableObjects.Firearms.Options
{
    [CreateAssetMenu(fileName = "New Pistol", menuName = "Game/Firearms/new Pistol", order = 0)]
    public class Pistol : AbstractFirearm
    {
        public Ammo ammo;
        public float fireRatePerSecond = 2.0f;
    }
}