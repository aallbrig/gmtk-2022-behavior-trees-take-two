using System;
using Model.Interfaces;
using Model.Interfaces.BattleSystem;
using MonoBehaviours.UI;
using UnityEngine;

namespace ScriptableObjects.Firearms
{
    [CreateAssetMenu(fileName = "New Firearm", menuName = "Game/Firearms/New firearm", order = 0)]
    public class Firearm : ScriptableObject, IFirearm, IControlButton, IWeapon
    {
        public event Action FirearmSelected;
        public event Action<IAmmo> FirearmFired;
        public event Action FirearmReloaded;

        public string firearmName = "Default firearm name";
        public float fireRatePerSecond = 2.0f;
        public float effectiveRange = 3.0f;
        public Ammo ammo;
        private IAmmo _ammo;
        private float _lastFiredTime;

        public bool CanFire() => Time.time - _lastFiredTime > fireRatePerSecond && _ammo.CanExpendBullet();

        public event Action WeaponFired;

        public float EffectiveRange => effectiveRange;

        public void Fire()
        {
            if (CanFire() == false) return;
            _lastFiredTime = Time.time;
            _ammo.ExpendBullet();
            FirearmFired?.Invoke(_ammo);
            WeaponFired?.Invoke();
        }
        public bool CanReload() => _ammo.CanReset();

        public void Reload()
        {
            if (CanReload() == false) return;
            _ammo.Reset();

            Debug.Log("Firearm has been reloaded");
            FirearmReloaded?.Invoke();
        }

        private void OnEnable()
        {
            // Instantiate so each firearm can manipulate its own ammo
            _ammo = Instantiate(ammo);
        }

        public string Label => firearmName;

        public string Description => $"{firearmName} fire rate {fireRatePerSecond} with ammo {_ammo}";

        public void Execute(IControlsMenu ctx)
        {
            FirearmSelected?.Invoke();
            ctx.ResetSecondaryMenu();
        }
    }
}