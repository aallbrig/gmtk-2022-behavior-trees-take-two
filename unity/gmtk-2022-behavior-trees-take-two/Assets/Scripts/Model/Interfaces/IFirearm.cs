using System;

namespace Model.Interfaces
{
    public interface IFirearm
    {
        public event Action<IAmmo> FirearmFired;
        public event Action FirearmReloaded;
        public float EffectiveRange { get; }

        public bool CanFire();
        public void Fire();

        public bool CanReload();
        public void Reload();
    }
}