using System;
using _Game.Scripts.Character;

namespace _Game.Scripts.Player {
    public class Equipment {
        public readonly EquipmentSlot Slot;
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