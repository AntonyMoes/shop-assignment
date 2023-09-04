using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Objects;
using _Game.Scripts.Shop;
using GeneralUtils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class ShopPanel : UIElement {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private InventoryPanelSlot _slotPrefab;

        private Action _onClose;
        private IInventoryObjectFactory _factory;
        private Inventory _playerInventory;
        private Inventory _shopInventory;
        private readonly List<InventoryPanelSlot> _slots = new();
        private float _sellPriceModifier;

        protected override void Init() {
            _closeButton.onClick.AddListener(OnClose);
        }

        public void Setup(Inventory playerInventory, Action onClose, IInventoryObjectFactory factory) {
            _playerInventory = playerInventory;
            _onClose = onClose;
            _factory = factory;
        }

        public void Load(float sellPriceModifier, Inventory shopInventory) {
            _sellPriceModifier = sellPriceModifier;
            _shopInventory = shopInventory;

            for (var i = 0; i < _shopInventory.Objects.Count; i++) {
                var slot = Instantiate(_slotPrefab, _slotParent);
                _slots.Add(slot);
                var index = i;
                slot.OnInventoryUpdate.Subscribe(obj => OnInventoryUpdate(index, obj));
            }
        }

        private void OnInventoryUpdate(int index, IInventoryObject inventoryObject) {
            if (inventoryObject != null) {
                if (_shopInventory.Objects[index] is {} obj) {
                    _playerInventory.Money.Value -= obj.Price;
                    _shopInventory.RemoveObject(index);
                }

                _playerInventory.Money.Value += Price.ApplyModifier(inventoryObject.Price, _sellPriceModifier);
                _shopInventory.AddObject(inventoryObject, index);
            } else {
                var obj = _shopInventory.Objects[index];
                _playerInventory.Money.Value -= obj.Price;
                _shopInventory.RemoveObject(index);
            }
        }

        protected override void PerformShow(Action onDone = null) {
            for (var i = 0; i < _shopInventory.Objects.Count; i++) {
                var slot = _slots[i];
                slot.Load(_shopInventory.Objects[i], _factory, true, 1f, CanDrop);
            }

            base.PerformShow(onDone);
        }

        private bool CanDrop(InventoryObjectUI newObject, InventoryObjectUI existingObject) {
            return CanDrop(true, newObject, existingObject);
        }

        public bool CanDropToInventory(InventoryObjectUI newObject, InventoryObjectUI existingObject) {
            return CanDrop(false, newObject, existingObject);
        }

        private bool CanDrop(bool shop, InventoryObjectUI newObject, InventoryObjectUI existingObject) {
            var fromLocalInventory = (shop ? _shopInventory : _playerInventory).Objects.Any(obj => obj == newObject.InventoryObject);
            if (fromLocalInventory) {
                return true;
            }

            var (boughtObject, soldObject) = shop ? (existingObject, newObject) : (newObject, existingObject);

            var needToPay = GetPrice(boughtObject, 1f) - GetPrice(soldObject, _sellPriceModifier);
            return needToPay <= _playerInventory.Money.Value;

            int GetPrice(InventoryObjectUI obj, float priceModifier) {
                return obj != null ? Price.ApplyModifier(obj.InventoryObject.Price, priceModifier) : 0;
            }
        }

        private void OnClose() {
            _onClose?.Invoke();
        }

        public override void Clear() {
            foreach (var slot in _slots) {
                slot.Clear();
                slot.OnInventoryUpdate.ClearSubscribers();
                Destroy(slot.gameObject);
            }
            _slots.Clear();

            foreach (var slot in _slots) {
                slot.Clear();
            }
        }
    }
}