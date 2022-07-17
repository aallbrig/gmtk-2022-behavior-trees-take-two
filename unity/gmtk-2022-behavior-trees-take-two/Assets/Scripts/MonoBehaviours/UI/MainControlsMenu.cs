using System;
using System.Collections.Generic;
using Model.Interfaces;
using Model.Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

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
            controlButtonContainer ??= transform; // use self as container, if none passed in
            RenderControlButtons();
        }

        private void ClearContainerChildren()
        {
            if (ContainerHasChildren() == false) return;

            foreach (Transform t in controlButtonContainer.transform)
                Destroy(t.gameObject);
        }

        private bool ContainerHasChildren() => controlButtonContainer.transform.childCount > 0;

        [ContextMenu("Render Control Buttons")]
        private void RenderControlButtons()
        {
            ClearContainerChildren();
            controlButtons.ForEach(controlButton =>
            {
                var soClone = Instantiate(controlButton);
                var instance = Instantiate(controlButtonPrefab, controlButtonContainer);
                instance.name = soClone.Description;
                var label = instance.GetComponent<ILabelSetter>();
                if (label != null)
                    label.SetLabel(soClone.Label);
                else
                    Debug.LogError("No label setter detected");
                var btn = instance.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(soClone.Execute);
                else
                    Debug.LogError("No button detected");
            });
        }
    }
}