using System;
using System.Collections.Generic;
using System.Linq;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.Interaction {
    public class InteractionComponent : MonoBehaviour {
        private readonly List<IInteractable> _interactables = new();

        private readonly UpdatedValue<IInteractable> _currentInteractable = new();
        public IUpdatedValue<IInteractable> CurrentInteractable => _currentInteractable;

        private void OnTriggerEnter2D(Collider2D trigger) {
            if (trigger.TryGetComponent<IInteractable>(out var interactable) && !_interactables.Contains(interactable)) {
                _interactables.Add(interactable);
                interactable.CanInteract.Value = true;
                _currentInteractable.Value = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D trigger) {
            if (trigger.TryGetComponent<IInteractable>(out var interactable) && _interactables.Contains(interactable)) {
                _interactables.Remove(interactable);
                interactable.CanInteract.Value = false;

                var newCurrent = _interactables.LastOrDefault();
                if (_currentInteractable.Value != newCurrent) {
                    _currentInteractable.Value = newCurrent;
                }
            }
        }
    }
}