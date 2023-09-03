using System;
using _Game.Scripts.Character;
using _Game.Scripts.Equipment;
using UnityEngine;

namespace _Game.Scripts.Objects {
    public class EquipmentObject : IInventoryObject, IEquipment{
        private readonly Equipment.Equipment _equipment;

        public Sprite Sprite { get; }
        public EquipmentSlot Slot => _equipment.Slot;

        public EquipmentObject(Sprite sprite, EquipmentSlot slot, Func<BodyPart> bodyPartFactory) {
            Sprite = sprite;
            _equipment = new Equipment.Equipment(slot, bodyPartFactory);
        }

        public BodyPart CreateBodyPart() => _equipment.CreateBodyPart();
    }
}