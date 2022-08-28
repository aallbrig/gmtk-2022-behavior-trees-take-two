using UnityEngine;

namespace Model.Player
{
    public interface ILocomotion
    {
        public Vector3 DesiredDirectionInWorld { get; }
    }
}