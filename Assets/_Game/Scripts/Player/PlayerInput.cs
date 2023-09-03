using System;
using _Game.Scripts.Scheduling;
using GeneralUtils;
using UnityEngine;
using Event = GeneralUtils.Event;

namespace _Game.Scripts.Player {
    public class PlayerInput : IFrameProcessor {
        private readonly IUpdatedValue<bool> _inputBlocked;
        private readonly UpdatedValue<Vector2> _directionalInput = new();
        public IUpdatedValue<Vector2> DirectionalInput => _directionalInput;

        private readonly Action _interactInput;
        public readonly Event InteractInput;

        public PlayerInput(IUpdatedValue<bool> inputBlocked) {
            _inputBlocked = inputBlocked;
            InteractInput = new Event(out _interactInput);
        }

        public void ProcessFrame(float deltaTime) {
            if (_inputBlocked.Value) {
                return;
            }

            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            var interactInput = Input.GetButtonDown("Interact");

            var directionalInput = new Vector2(horizontalInput, verticalInput).normalized;
            if (_directionalInput.Value != directionalInput) {
                _directionalInput.Value = directionalInput;
            }

            if (interactInput) {
                _interactInput();
            }
        }
    }
}