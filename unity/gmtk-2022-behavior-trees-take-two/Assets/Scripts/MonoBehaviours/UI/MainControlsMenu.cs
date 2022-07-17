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
        public List<AbstractControlButton> controlButtons = new List<AbstractControlButton>();

        private void Start()
        {
            CurrentState ??= new NothingSelectedState();
            RenderControlButtons();
        }

        [ContextMenu("Render Control Buttons")]
        private void RenderControlButtons()
        {
            controlButtons.ForEach(controlButton =>
            {
                var soClone = ScriptableObject.Instantiate(controlButton);
                var instance = Instantiate(controlButtonPrefab, controlButtonContainer);
                instance.name = soClone.Description;
                var btn = instance.GetComponent<ILabelSetter>();
                if (btn != null)
                    btn.SetLabel(soClone.Label);
                else
                    Debug.LogError("No label setter detected");
            });
        }
    }
}