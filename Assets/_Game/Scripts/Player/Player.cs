using _Game.Scripts.Character;
using _Game.Scripts.Equipment;
using _Game.Scripts.Objects;
using _Game.Scripts.Scheduling;
using GeneralUtils;

namespace _Game.Scripts.Player {
    public class Player {
        private readonly IUpdatedValue<bool> _blocker;
        private const int InventorySize = 10;

        private readonly PlayerInput _playerInput;
        public readonly Inventory Inventory;
        public readonly EquipmentController EquipmentController;

        private ICharacterController _characterController;

        public Player(IScheduler scheduler, IUpdatedValue<bool> blocker) {
            _blocker = blocker;
            _playerInput = new PlayerInput(_blocker);
            scheduler.RegisterFrameProcessor(_playerInput);
            _playerInput.InteractInput.Subscribe(OnInteract);
            EquipmentController = new EquipmentController();
            Inventory = new Inventory(InventorySize);
        }

        public void SetController(ICharacterController characterController) {
            if (_characterController != null) {
                _characterController.SetInteractionBlocker(null);
                _playerInput.DirectionalInput.Unsubscribe(_characterController.SetMovementDirection);
                EquipmentController.OnEquipmentActive.Unsubscribe(_characterController.OnEquipmentActive);
            }

            _characterController = characterController;

            if (_characterController != null) {
                _characterController.SetInteractionBlocker(_blocker);
                _playerInput.DirectionalInput.Subscribe(_characterController.SetMovementDirection);
                EquipmentController.OnEquipmentActive.Subscribe(_characterController.OnEquipmentActive);
                EquipmentController.SetDefaultEquipmentCollection(_characterController.ProvideDefaultEquipment());
            }
        }

        private void OnInteract() {
            if (_characterController.CurrentInteractable.Value is { } interactable) {
                interactable.Interact();
            }
        }
    }
}