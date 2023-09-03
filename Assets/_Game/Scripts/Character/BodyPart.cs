using _Game.Scripts.Equipment;
using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts.Character {
    public class BodyPart : MonoBehaviour {
        [SerializeField] private Animator _animator;
        [SerializeField] private EquipmentSlot _slot;
        public EquipmentSlot Slot => _slot;

        private Vector2 _lastDirection;
        private float _lastVelocity;

        public void SetDirection(Vector2 direction) {
            var magnitude = direction.magnitude;
            SetVelocityValues(magnitude);
            if (magnitude <= 0) {
                return;
            }

            SetDirectionValues(direction);
        }

        private void SetVelocityValues(float velocity) {
            _lastVelocity = velocity;
            _animator.SetFloat(AnimationParameter.Velocity, velocity);
        }

        private void SetDirectionValues(Vector2 direction) {
            _lastDirection = direction;
            _animator.SetFloat(AnimationParameter.HorizontalDirection, direction.x);
            _animator.SetFloat(AnimationParameter.VerticalDirection, direction.y);
        }

        public void ResetAnimationTime() {
            _animator.Play(0, -1, 0);
        }

        public void ResetValuesFromOther(BodyPart other) {
            SetVelocityValues(other._lastVelocity);
            SetDirectionValues(other._lastDirection);
        }

#if UNITY_EDITOR
        public void SetAnimator(Animator animator) {
            _animator = animator;
        }
#endif
    }
}