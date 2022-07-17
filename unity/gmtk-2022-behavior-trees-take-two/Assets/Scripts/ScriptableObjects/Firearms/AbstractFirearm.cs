using UnityEngine;

namespace ScriptableObjects.Firearms
{
    public abstract class AbstractFirearm : ScriptableObject
    {
        public abstract Ammo GetAmmo();
        public abstract void Fire();
    }
}