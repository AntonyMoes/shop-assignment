using UnityEngine;

namespace _Game.Scripts.UI {
    public interface IInteractPanelPresenter {
        public void ShowInteractPanel(Transform anchor);
        public void HideInteractPanel(Transform anchor);
    }
}