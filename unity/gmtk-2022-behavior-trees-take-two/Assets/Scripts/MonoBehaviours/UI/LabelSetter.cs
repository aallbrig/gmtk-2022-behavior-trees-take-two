using Model.Interfaces;
using UnityEngine;
using TMPro;

namespace MonoBehaviours.UI
{
    public class LabelSetter : MonoBehaviour, ILabelSetter
    {
        public TextMeshProUGUI targetText;

        public void SetLabel(string label)
        {
            targetText.text = label;
        }
    }
}