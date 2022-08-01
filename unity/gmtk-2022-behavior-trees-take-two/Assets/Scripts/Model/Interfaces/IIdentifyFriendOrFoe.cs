using System;
using UnityEngine;

namespace Model.Interfaces
{
    public enum FriendOrFoeResponse
    {
        Friendly, Hostile, NoResponse
    }
    public interface IIdentifyFriendOrFoe
    {
        public FriendOrFoeResponse ProcessInterrogationSignal();
    }
}