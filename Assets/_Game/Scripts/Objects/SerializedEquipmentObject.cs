using _Game.Scripts.Character;
using _Game.Scripts.Equipment;
using UnityEngine;

namespace _Game.Scripts.Objects {
    [CreateAssetMenu]
    public class SerializedEquipmentObject : SerializedInventoryObject {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private EquipmentSlot _slot;
        [SerializeField] private BodyPart _bodyPart;

        public override string Name => name;

        public override IInventoryObject CreateInventoryObject() {
            return new EquipmentObject(_sprite, _slot, () => Instantiate(_bodyPart));
        }
    }
}