using System;
using UnityEngine;

namespace Model.Interfaces
{
    public interface IGameAgent
    {
        public Guid ID { get; }
        public GameObject GameObject { get; }
    }
}