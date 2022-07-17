using System;
using System.Collections.Generic;
using Model.Interfaces;
using Model.Player;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class MainControlsMenu : MonoBehaviour, IPlayerActionFSM
    {
        public IPlayerActionState CurrentState { get; private set; }
        public Transform controlButtonContainer;
        public GameObject controlButtonPrefab;
        private Transform _controlButtonContainer;
        public List<AbstractControlButton> controlButtons = new List<AbstractControlButton>();

        private void Start()
        {
            _controlButtonContainer ??= controlButtonContainer ? controlButtonContainer : throw new ArgumentNullException();
            CurrentState ??= new NothingSelectedState();
            RenderControlButtons();
        }

        private void RenderControlButtons()
        {
            controlButtons.ForEach(controlButton =>
            {
                var instance = Instantiate(controlButtonPrefab, _controlButtonContainer);
                var btn = instance.GetComponent<ILabelSetter>();
                if (btn != null)
                    btn.SetLabel(controlButton.Label);
                else
                    Debug.LogError("No label setter detected");
            });
        }
    }
}