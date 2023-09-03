using System;
using System.Collections.Generic;
using _Game.Scripts.Objects;
using GeneralUtils.UI;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class InventoryPanel : UIElement {
        [SerializeField] private Transform _slotParent;
        [SerializeField] private InventoryPanelSlot _slotPrefab;
        [SerializeField] private GameObject _moneyGroup;
        [SerializeField] private TextMeshProUGUI _money;

        private Inventory _inventory;
        private IInventoryObjectFactory _factory;
        private readonly List<InventoryPanelSlot> _slots = new();
        private float _priceModifier;

        public void Setup(Inventory inventory, IInventoryObjectFactory inventoryObjectFactory) {
            _inventory = inventory;
            _factory = inventoryObjectFactory;

            for (var i = 0; i < _inventory.Objects.Count; i++) {
                var slot = Instantiate(_slotPrefab, _slotParent);
                _slots.Add(slot);
                var index = i;
                slot.OnInventoryUpdate.Subscribe(obj => OnInventoryUpdate(index, obj));
            }

            _inventory.Money.Subscribe(OnMoneyChange, true);
        }

        private void OnMoneyChange(int value) {
            _money.text = value.ToString();
        }

        public void Load(bool showMoney, float priceModifier = 1f) {
            _moneyGroup.SetActive(showMoney);
            _priceModifier = priceModifier;

            for (var i = 0; i < _inventory.Objects.Count; i++) {
                var slot = _slots[i];
                slot.Load(_inventory.Objects[i], _factory, showMoney, priceModifier);
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

        public override void Clear() {
            foreach (var slot in _slots) {
                slot.Clear();
            }
        }
    }
}