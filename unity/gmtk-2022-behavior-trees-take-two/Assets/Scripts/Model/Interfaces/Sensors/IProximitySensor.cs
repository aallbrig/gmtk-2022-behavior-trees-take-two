using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Interfaces.Sensors
{
    public interface IProximitySensor: ISensor
    {
        public List<GameObject> Friendlies { get; }
        public List<GameObject> Enemies { get; }
        public List<GameObject> CoverPoints { get; }

        public void SenseForFriendlies();
        public void SenseForEnemies();
        public void SenseForCoverPoints();
    }
}