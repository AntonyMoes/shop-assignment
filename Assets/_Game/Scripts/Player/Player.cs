using _Game.Scripts.Character;
using _Game.Scripts.Scheduling;

namespace _Game.Scripts.Player {
    public class Player {
        private readonly PlayerInput _playerInput;
        private readonly PlayerEquipment _playerEquipment;

        private ICharacterController _characterController;

        public Player(IScheduler scheduler, IInputBlocker inputBlocker) {
            _playerInput = new PlayerInput(inputBlocker);
            scheduler.RegisterFrameProcessor(_playerInput);
            _playerInput.InteractInput.Subscribe(OnInteract);
            _playerEquipment = new PlayerEquipment();
        }

        public void SetController(ICharacterController characterController) {
            if (_characterController != null) {
                _playerInput.DirectionalInput.Unsubscribe(_characterController.SetMovementDirection);
                _playerEquipment.OnEquipmentActive.Unsubscribe(_characterController.OnEquipmentActive);
            }

            _characterController = characterController;

            if (_characterController != null) {
                _playerInput.DirectionalInput.Subscribe(_characterController.SetMovementDirection);
                _playerEquipment.OnEquipmentActive.Subscribe(_characterController.OnEquipmentActive);
                _playerEquipment.SetDefaultEquipmentCollection(_characterController.ProvideDefaultEquipment());
            }
        }

        private void OnInteract() {
            if (_characterController.CurrentInteractable.Value is { } interactable) {
                interactable.Interact();
            }
        }
    }
}