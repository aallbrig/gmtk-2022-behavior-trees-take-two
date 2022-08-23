using System;
using Model.Interfaces.BattleSystem;
using ScriptableObjects.Firearms;
using UnityEngine;

namespace MonoBehaviours.BattleSystem
{
    public class WeaponsUser : MonoBehaviour, IWeaponsUser
    {
        public event Action NoValidWeapon;

        public IWeapon Weapon { get; set; }
        public Firearm fireArm;

        private void Start()
        {
            if (fireArm != null)
                Weapon = fireArm;
        }

        public void CommandFireWeapon()
        {
            if (Weapon == null)
                NoValidWeapon?.Invoke();
        }
    }
}