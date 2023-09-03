using _Game.Scripts.Interaction;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.Character {
    public interface ICharacterController {
        public void SetMovementDirection(Vector2 direction);
        public IUpdatedValue<IInteractable> CurrentInteractable { get; }
    }
}