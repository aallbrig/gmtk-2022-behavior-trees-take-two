using System;

namespace Model.Interfaces.BattleSystem
{
    public interface IWeapon
    {
        public event Action WeaponFired;
        public float EffectiveRange { get; }
        public void Fire();
    }
}