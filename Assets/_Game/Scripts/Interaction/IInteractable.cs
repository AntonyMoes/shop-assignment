using GeneralUtils;

namespace _Game.Scripts.Interaction {
    public interface IInteractable {
        public UpdatedValue<bool> CanInteract { get; }
        public void Interact();
    }
}