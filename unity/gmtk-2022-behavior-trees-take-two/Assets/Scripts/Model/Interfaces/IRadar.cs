using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Interfaces
{
    public interface IRadar
    {
        public event Action<GameObject> ObjectDetected;
        public IEnumerable<GameObject> DetectObjects();
    }
}