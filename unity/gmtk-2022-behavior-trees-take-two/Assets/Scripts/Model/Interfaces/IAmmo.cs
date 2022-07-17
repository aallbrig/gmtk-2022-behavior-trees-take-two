using System;

namespace Model.Interfaces
{
    public interface IAmmo
    {
        public event Action AmmoClipReset;
        public event Action BulletExpended;

        public void ExpendBullet();
        public bool CanExpendBullet();

        public void Reset();
        public bool CanReset();
    }
}