using _Game.Scripts.UI;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.Interaction {
    public abstract class Interactable : MonoBehaviour, IInteractable {
        [SerializeField] private Transform _interactPanelAnchor;

        private IInteractPanelPresenter _interactPanelPresenter;
        public UpdatedValue<bool> CanInteract { get; } = new();

        public void Init(IInteractPanelPresenter interactPanelPresenter) {
            _interactPanelPresenter = interactPanelPresenter;
            CanInteract.Subscribe(OnCanInteract);
        }

        private void OnCanInteract(bool canInteract) {
            if (canInteract) {
                _interactPanelPresenter.ShowInteractPanel(_interactPanelAnchor);
            } else {
                _interactPanelPresenter.HideInteractPanel(_interactPanelAnchor);
            }
        }

        public abstract void Interact();
    }
}