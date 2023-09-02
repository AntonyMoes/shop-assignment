using UnityEngine;

namespace _Game.Scripts.Character {
    public class CharacterController : MonoBehaviour {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed;

        [SerializeField] private BodyPart _body;

        private Vector2 _direction;

        public void SetMovementDirection(Vector2 direction) {
            _direction = direction;
            _body.SetDirection(direction);
        }

        private void FixedUpdate() {
            _rb.velocity = _direction * _speed;
        }
    }
}