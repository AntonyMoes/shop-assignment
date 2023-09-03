using System;
using System.Collections.Generic;
using GeneralUtils;

namespace _Game.Scripts.Player {
    public class PlayerEquipment {
        private readonly Dictionary<EquipmentSlot, Equipment> _defaultEquipment = new();

        private readonly Dictionary<EquipmentSlot, Equipment> _equipment = new();
        public IDictionary<EquipmentSlot, Equipment> Equipment => _equipment;

        private readonly Action<Equipment, bool> _onEquipmentActive;
        public readonly Event<Equipment, bool> OnEquipmentActive;

        public PlayerEquipment() {
            OnEquipmentActive = new Event<Equipment, bool>(out _onEquipmentActive);
        }

        public void SetDefaultEquipmentCollection(Equipment[] defaultEquipment) {
            foreach (var slot in _defaultEquipment.Keys) {
                RemoveDefaultEquipment(slot);
            }
            _defaultEquipment.Clear();

            foreach (var equipment in defaultEquipment) {
                _defaultEquipment.Add(equipment.Slot, equipment);
                SetDefaultEquipment(equipment.Slot);
            }
        }

        public void SetEquipment(Equipment equipment) {
            RemoveDefaultEquipment(equipment.Slot);
            RemoveEquipment(equipment.Slot, false);

            _equipment.Add(equipment.Slot, equipment);
            _onEquipmentActive(equipment, true);
        }

        public void RemoveEquipment(EquipmentSlot slot) {
            RemoveEquipment(slot, true);
        }

        private void RemoveEquipment(EquipmentSlot slot, bool equipDefault) {
            if (!_equipment.TryGetValue(slot, out var equipment)) {
                return;
            }

            _onEquipmentActive(equipment, false);
            _equipment.Remove(slot);

            if (equipDefault) {
                SetDefaultEquipment(slot);
            }
        }

        private void SetDefaultEquipment(EquipmentSlot slot) {
            if (_defaultEquipment.TryGetValue(slot, out var defaultEquipment)) {
                _onEquipmentActive(defaultEquipment, true);
            }
        }

        private void RemoveDefaultEquipment(EquipmentSlot slot) {
            if (!_equipment.ContainsKey(slot) && _defaultEquipment.TryGetValue(slot, out var defaultEquipment)) {
                _onEquipmentActive(defaultEquipment, false);
            }
        }
    }
}