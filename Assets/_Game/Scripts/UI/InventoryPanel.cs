using System;
using System.Collections.Generic;
using _Game.Scripts.Objects;
using GeneralUtils.UI;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class InventoryPanel : UIElement {
        [SerializeField] private Transform _slotParent;
        [SerializeField] private InventoryPanelSlot _slotPrefab;

        private Inventory _inventory;
        private IInventoryObjectFactory _factory;
        private readonly List<InventoryPanelSlot> _slots = new();

        public void Setup(Inventory inventory, IInventoryObjectFactory inventoryObjectFactory) {
            _inventory = inventory;
            _factory = inventoryObjectFactory;

            for (var i = 0; i < _inventory.Objects.Count; i++) {
                var slot = Instantiate(_slotPrefab, _slotParent);
                _slots.Add(slot);
                var index = i;
                slot.OnInventoryUpdate.Subscribe(obj => OnInventoryUpdate(index, obj));
            }
        }

        private void OnInventoryUpdate(int index, IInventoryObject inventoryObject) {
            if (inventoryObject != null) {
                if (_inventory.Objects[index] != null) {
                    _inventory.RemoveObject(index);
                }

                _inventory.AddObject(inventoryObject, index);
            } else {
                _inventory.RemoveObject(index);
            }
        }

        protected override void PerformShow(Action onDone = null) {
            for (var i = 0; i < _inventory.Objects.Count; i++) {
                var slot = _slots[i];
                slot.Load(_inventory.Objects[i], _factory);
            }

            base.PerformShow(onDone);
        }

        public override void Clear() {
            foreach (var slot in _slots) {
                slot.Clear();
            }
        }
    }
}