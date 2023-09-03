using System;
using System.Collections.Generic;
using System.Linq;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.Interaction {
    public class InteractionComponent : MonoBehaviour {
        private readonly List<IInteractable> _interactables = new();

        private IUpdatedValue<bool> _interactionBlocked;
        private readonly UpdatedValue<IInteractable> _currentInteractable = new();
        public IUpdatedValue<IInteractable> CurrentInteractable => _currentInteractable;

        public void SetInteractionBlocker(IUpdatedValue<bool> interactionBlocked) {
            _interactionBlocked?.Unsubscribe(OnInteractionBlocked);
            _interactionBlocked = interactionBlocked;
            _interactionBlocked?.Subscribe(OnInteractionBlocked);
        }

        private void OnInteractionBlocked(bool blocked) {
            if (blocked) {
                if (_currentInteractable.Value is {} interactable) {
                    interactable.CanInteract.Value = false;
                    _currentInteractable.Value = null;
                }
            } else {
                SetCurrentFromRegistered();
            }
        }

        private void OnTriggerEnter2D(Collider2D trigger) {
            if (trigger.TryGetComponent<IInteractable>(out var interactable) && !_interactables.Contains(interactable)) {
                _interactables.Add(interactable);

                if (!(_interactionBlocked?.Value ?? false)) {
                    interactable.CanInteract.Value = true;
                    _currentInteractable.Value = interactable;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D trigger) {
            if (trigger.TryGetComponent<IInteractable>(out var interactable) && _interactables.Contains(interactable)) {
                _interactables.Remove(interactable);

                if (!(_interactionBlocked?.Value ?? false)) {
                    interactable.CanInteract.Value = false;
                    SetCurrentFromRegistered();
                }
            }
        }

        private void SetCurrentFromRegistered() {
            var newCurrent = _interactables.LastOrDefault();
            if (_currentInteractable.Value != newCurrent) {
                if (newCurrent != null) {
                    newCurrent.CanInteract.Value = true;
                }
                _currentInteractable.Value = newCurrent;
            }
        }
    }
}