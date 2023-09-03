using _Game.Scripts.Character;

namespace _Game.Scripts.Equipment {
    public interface IEquipment {
        public EquipmentSlot Slot { get; }
        public BodyPart CreateBodyPart();
    }
}