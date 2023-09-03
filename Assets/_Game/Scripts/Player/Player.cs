using _Game.Scripts.Character;
using _Game.Scripts.Scheduling;

namespace _Game.Scripts.Player {
    public class Player {
        private readonly PlayerInput _playerInput;

        private ICharacterController _characterController;
        public Player(IScheduler scheduler, IInputBlocker inputBlocker) {
            _playerInput = new PlayerInput(inputBlocker);
            scheduler.RegisterFrameProcessor(_playerInput);
            _playerInput.InteractInput.Subscribe(OnInteract);
        }

        public void SetController(ICharacterController characterController) {
            if (_characterController != null) {
                _playerInput.DirectionalInput.Unsubscribe(_characterController.SetMovementDirection);
            }

            _characterController = characterController;

            if (_characterController != null) {
                _playerInput.DirectionalInput.Subscribe(_characterController.SetMovementDirection);
            }
        }

        private void OnInteract() {
            if (_characterController.CurrentInteractable.Value is { } interactable) {
                interactable.Interact();
            }
        }
    }
}