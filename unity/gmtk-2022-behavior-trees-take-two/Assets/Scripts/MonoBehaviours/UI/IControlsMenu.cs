using System.Collections.Generic;

namespace MonoBehaviours.UI
{
    public interface IControlsMenu
    {
        public void PopulatePrimaryMenu(List<IControlButton> buttons);
        public void PopulateSecondaryMenu(List<IControlButton> buttons);
        public void ResetSecondaryMenu();
    }
}