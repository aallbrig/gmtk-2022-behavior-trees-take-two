using UnityEngine;

namespace ScriptableObjects.Firearms
{
    public abstract class AbstractFirearm : ScriptableObject
    {
        public string Name = "Default firearm name";
        public abstract Ammo GetAmmo();
        public abstract void Fire();
    }
}