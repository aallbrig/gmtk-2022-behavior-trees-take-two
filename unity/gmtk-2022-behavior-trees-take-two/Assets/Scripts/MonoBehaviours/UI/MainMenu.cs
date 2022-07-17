using System;
using Model.Player;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class MainMenu : MonoBehaviour, IPlayerActionFSM
    {
        public IPlayerActionState CurrentState { get; private set; }

        private void Start()
        {
            CurrentState ??= new NothingSelectedState();
        }
    }
}