using _Game.Scripts.Equipment;
using _Game.Scripts.Interaction;
using _Game.Scripts.Player;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.Character {
    public interface ICharacterController {
        public void SetMovementDirection(Vector2 direction);
        public IUpdatedValue<IInteractable> CurrentInteractable { get; }
        public void SetInteractionBlocker(IUpdatedValue<bool> interactionBlocked);
        public void OnEquipmentActive(IEquipment equipment, bool active);
        public IEquipment[] ProvideDefaultEquipment();
    }
}