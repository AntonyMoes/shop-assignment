using System;
using _Game.Scripts.Character;

namespace _Game.Scripts.Equipment {
    public class Equipment : IEquipment {
        public EquipmentSlot Slot { get; }
        private readonly Func<BodyPart> _bodyPartFactory;

        public Equipment(EquipmentSlot slot, Func<BodyPart> bodyPartFactory) {
            Slot = slot;
            _bodyPartFactory = bodyPartFactory;
        }

        public BodyPart CreateBodyPart() {
            return _bodyPartFactory();
        }
    }
}