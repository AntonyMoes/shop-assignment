using UnityEngine;

namespace _Game.Scripts {
    public class CharacterController : MonoBehaviour {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed;

        private Vector2 _direction;

        public void SetMovementDirection(Vector2 direction) {
            _direction = direction;
        }

        private void FixedUpdate() {
            _rb.velocity = _direction * _speed;
        }
    }
}