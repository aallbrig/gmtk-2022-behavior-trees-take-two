using System;
using Model.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Firearms
{
    [CreateAssetMenu(fileName = "New Ammo Stats", menuName = "Game/Firearms/new Ammo", order = 0)]
    public class Ammo : ScriptableObject, IAmmo
    {
        public event Action AmmoClipReset;
        public event Action BulletExpended;

        public int bulletsPerClip = 7;
        public int maxClipCarryCount = 4;
        private int _currentClipBulletCount;
        private int _currentClipCount;

        private void OnEnable()
        {
            _currentClipCount = maxClipCarryCount;
            _currentClipBulletCount = bulletsPerClip;
        }


        public void ExpendBullet()
        {
            if (CanExpendBullet() == false) return;

            _currentClipCount--;
            BulletExpended?.Invoke();
        }

        public bool CanExpendBullet() => _currentClipCount > 0;

        public void Reset()
        {
            if (CanReset() == false) return;

            _currentClipCount--;
            _currentClipBulletCount = bulletsPerClip;

            AmmoClipReset?.Invoke();
        }

        public bool CanReset() => _currentClipCount > 0;
    }
}