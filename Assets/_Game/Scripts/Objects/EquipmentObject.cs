using System;
using _Game.Scripts.Character;
using _Game.Scripts.Equipment;
using UnityEngine;

namespace _Game.Scripts.Objects {
    public class EquipmentObject : IInventoryObject, IEquipment{
        private readonly Equipment.Equipment _equipment;

        public Sprite Sprite { get; }
        public int Price { get; }
        public EquipmentSlot Slot => _equipment.Slot;

        public EquipmentObject(int price, Sprite sprite, EquipmentSlot slot, Func<BodyPart> bodyPartFactory) {
            Price = price;
            Sprite = sprite;
            _equipment = new Equipment.Equipment(slot, bodyPartFactory);
        }

        public BodyPart CreateBodyPart() => _equipment.CreateBodyPart();
    }
}