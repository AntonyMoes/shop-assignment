using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Objects;
using _Game.Scripts.Shop;
using GeneralUtils;
using GeneralUtils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class ShopPanel : UIElement {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Transform _slotParent;
        [SerializeField] private InventoryPanelSlot _slotPrefab;
        [SerializeField] private TextMeshProUGUI _totalText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _buyButtonText;

        [SerializeField] private string _buyString;
        [SerializeField] private string _sellString;

        private Action _onClose;
        private IInventoryObjectFactory _factory;
        private Inventory _playerInventory;
        private Inventory _shopInventory;
        private readonly List<InventoryPanelSlot> _slots = new();
        private float _sellPriceModifier;

        private ShopTransaction _transaction;

        protected override void Init() {
            _closeButton.onClick.AddListener(OnClose);
            _buyButton.onClick.AddListener(OnBuy);
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
            var fromPlayer = _shopInventory.Objects.All(obj => obj != inventoryObject);
            if (inventoryObject != null) {
                if (_shopInventory.Objects[index] is {} obj) {
                    _shopInventory.RemoveObject(index);
                    if (fromPlayer) {
                        _transaction.AddTransfer(obj, true);
                    }
                }

                _shopInventory.AddObject(inventoryObject, index);
                if (fromPlayer) {
                    _transaction.AddTransfer(inventoryObject, false);
                }
            } else {
                var obj = _shopInventory.Objects[index];
                _shopInventory.RemoveObject(index);
                _transaction.AddTransfer(obj, true);
            }
        }

        protected override void PerformShow(Action onDone = null) {
            for (var i = 0; i < _shopInventory.Objects.Count; i++) {
                var slot = _slots[i];
                slot.Load(_shopInventory.Objects[i], _factory, true, 1f);
            }

            _buyButton.interactable = false;
            _transaction = new ShopTransaction(_playerInventory, _shopInventory, _sellPriceModifier);
            _transaction.TotalCost.Subscribe(OnTransactionCostChange, true);

            base.PerformShow(onDone);
        }

        private void OnTransactionCostChange(int cost) {
            if (cost >= 0) {
                _totalText.text = cost.ToString();
                _buyButtonText.text = _buyString;
            } else {
                _totalText.text = (-cost).ToString();
                _buyButtonText.text = _sellString;
            }

            _buyButton.interactable = cost <= _playerInventory.Money.Value && _transaction.HasTransfers;
        }

        private void OnBuy() {
            _playerInventory.Money.Value -= _transaction.TotalCost.Value;
            _transaction = null;

            OnClose();
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

            _transaction?.Rollback();
            _transaction = null;

            foreach (var slot in _slots) {
                slot.Clear();
            }
        }
    }
}