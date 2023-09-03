using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Interaction;
using _Game.Scripts.Player;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.Character {
    public class CharacterController : MonoBehaviour, ICharacterController {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed;
        [SerializeField] private InteractionComponent _interactionComponent;
        [SerializeField] private BodyPart[] _defaultParts;
        [SerializeField] private Transform _bodyPartParent;
        public IUpdatedValue<IInteractable> CurrentInteractable => _interactionComponent.CurrentInteractable;

        private readonly Dictionary<Equipment, BodyPart> _equipment = new ();

        private Vector2 _direction;

        public void SetMovementDirection(Vector2 direction) {
            _direction = direction;

            foreach (var part in _equipment.Values) {
                part.SetDirection(direction);
            }
        }

        private void FixedUpdate() {
            _rb.velocity = _direction * _speed;
        }

        public void OnEquipmentActive(Equipment equipment, bool active) {
            if (active) {
                var bodyPart = equipment.CreateBodyPart();
                var partTransform = bodyPart.transform;
                partTransform.SetParent(_bodyPartParent);
                partTransform.localScale = Vector3.one;
                partTransform.localPosition = Vector3.zero;
                _equipment.Add(equipment, bodyPart);
            } else {
                Destroy(_equipment[equipment].gameObject);
                _equipment.Remove(equipment);
            }
        }

        public Equipment[] ProvideDefaultEquipment() {
            return _defaultParts
                .Select(part => new Equipment(part.Slot, () => Instantiate(part)))
                .ToArray();
        }
    }
}