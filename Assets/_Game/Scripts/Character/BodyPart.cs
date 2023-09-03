using _Game.Scripts.Player;
using UnityEngine;

namespace _Game.Scripts.Character {
    public class BodyPart : MonoBehaviour {
        [SerializeField] private Animator _animator;
        [SerializeField] private EquipmentSlot _slot;
        public EquipmentSlot Slot => _slot;

        public void SetDirection(Vector2 direction) {
            var magnitude = direction.magnitude;
            _animator.SetFloat(AnimationParameter.Velocity, magnitude);
            if (magnitude <= 0) {
                return;
            }

            _animator.SetFloat(AnimationParameter.HorizontalDirection, direction.x);
            _animator.SetFloat(AnimationParameter.VerticalDirection, direction.y);
        }

#if UNITY_EDITOR
        public void SetAnimator(Animator animator) {
            _animator = animator;
        }
#endif
    }
}