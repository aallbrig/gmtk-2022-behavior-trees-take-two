using System;

namespace Model.Interfaces.BattleSystem
{
    public interface IWeaponsUser
    {
        public event Action NoValidWeapon;
        public IWeapon Weapon { get; set; }
        public void CommandFireWeapon();
    }
}