using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;
using Model.Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviours.UI
{
    public class PlayerControlsMenu : MonoBehaviour, IPlayerActionFSM, IControlsMenu
    {
        public IPlayerActionState CurrentState { get; private set; }
        public Transform primaryControlsContainer;
        public Transform secondaryControlsContainer;
        public GameObject controlButtonPrefab;
        public List<AbstractControlButton> primaryMenuControlButtons = new List<AbstractControlButton>();
        public List<AbstractControlButton> secondaryMenuControlButtons = new List<AbstractControlButton>();
        [Header("DEBUGGING")]
        [SerializeField]
        private List<Transform> runtimePrimaryMenuControlButtons = new List<Transform>();
        [SerializeField]
        private List<Transform> runtimeSecondaryControlButtons = new List<Transform>();

        private void Start()
        {
            CurrentState ??= new NothingSelectedState();
            var parentTransform = transform;
            primaryControlsContainer ??= new GameObject { transform = { parent = parentTransform }}.transform; // use self as container, if none passed in
            secondaryControlsContainer ??= new GameObject { transform = { parent = parentTransform }}.transform; // use self as container, if none passed in

            SyncControlMenus();
        }

        [ContextMenu("Render control buttons")]
        private void SyncControlMenus()
        {
            PopulatePrimaryMenu(primaryMenuControlButtons.ToList<IControlButton>());
            PopulateSecondaryMenu(secondaryMenuControlButtons.ToList<IControlButton>());
        }

        public void PopulatePrimaryMenu(List<IControlButton> buttons)
        {
            DeleteChildrenInContainer(primaryControlsContainer);
            var transforms = RenderControlButtons(primaryControlsContainer, buttons);

            runtimePrimaryMenuControlButtons.Clear();
            runtimePrimaryMenuControlButtons.AddRange(transforms);
        }
        public void PopulateSecondaryMenu(List<IControlButton> buttons)
        {
            DeleteChildrenInContainer(secondaryControlsContainer);
            var transforms = RenderControlButtons(secondaryControlsContainer, buttons);

            runtimeSecondaryControlButtons.Clear();
            runtimeSecondaryControlButtons.AddRange(transforms);
        }
        public void ResetSecondaryMenu()
        {
            DeleteChildrenInContainer(secondaryControlsContainer);
        }

        private void DeleteChildrenInContainer(Transform container)
        {
            foreach (Transform t in container.transform)
                Destroy(t.gameObject);
        }

        private IEnumerable<Transform> RenderControlButtons(Transform container, IEnumerable<IControlButton> buttons)
        {
            return buttons.Select(controlButton =>
            {
                var instance = Instantiate(controlButtonPrefab, container);

                instance.name = controlButton.Description;
                var label = instance.GetComponent<ILabelSetter>();
                if (label != null)
                    label.SetLabel(controlButton.Label);
                else
                    Debug.LogError("No label setter detected");

                // TODO: add this to model by introducing an interface?
                var btn = instance.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(() => controlButton.Execute(this));
                else
                    Debug.LogError("No button detected");

                return instance.transform;
            });
        }
    }
}