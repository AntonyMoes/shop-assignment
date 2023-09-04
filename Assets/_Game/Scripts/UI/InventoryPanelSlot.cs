using System;
using _Game.Scripts.Objects;
using _Game.Scripts.UI.DragAndDrop;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class InventoryPanelSlot : MonoBehaviour {
        [SerializeField] private DropComponent _dropComponent;

        private readonly Action<IInventoryObject> _onInventoryUpdate;
        public readonly Event<IInventoryObject> OnInventoryUpdate;

        private Func<InventoryObjectUI, InventoryObjectUI, bool> _canDrop;
        private InventoryObjectUI _inventoryObject;
        private bool _showPrice;
        private float _priceModifier;

        public InventoryPanelSlot() {
            OnInventoryUpdate = new Event<IInventoryObject>(out _onInventoryUpdate);
        }

        private void Awake() {
            _dropComponent.Init(CanDrop);
            _dropComponent.OnDropped.Subscribe(OnDropped);
        }

        public void Load(IInventoryObject equipment, IInventoryObjectFactory inventoryObjectFactory, bool showPrice,
            float priceModifier, Func<InventoryObjectUI, InventoryObjectUI, bool> canDrop = null) {
            _canDrop = canDrop;
            _showPrice = showPrice;
            _priceModifier = priceModifier;

            if (equipment != null) {
                _inventoryObject = inventoryObjectFactory.CreateInventoryObject(transform, equipment, _dropComponent);
                _inventoryObject.SetShowPrice(showPrice, priceModifier);
            }
        }

        public void Clear() {
            if (_inventoryObject != null) {
                Destroy(_inventoryObject.gameObject);
            }
        }

        private bool CanDrop(DragComponent component, DragComponent otherComponent) {
            if (!component.TryGetComponent<InventoryObjectUI>(out var inventoryObject)) {
                return false;
            }

            var otherObject = otherComponent != null ? otherComponent.GetComponent<InventoryObjectUI>() : null;
            return _canDrop?.Invoke(inventoryObject, otherObject) ?? true;
        }

        private void OnDropped(DragComponent dragComponent) {
            var inventoryObjectUI = dragComponent != null ? dragComponent.GetComponent<InventoryObjectUI>() : null;
            if (inventoryObjectUI != null) {
                inventoryObjectUI.SetShowPrice(_showPrice, _priceModifier);
            }

            var equipmentObject = inventoryObjectUI != null
                ? inventoryObjectUI.InventoryObject
                : null;

            _inventoryObject = inventoryObjectUI;
            _onInventoryUpdate(equipmentObject);
        }
    }
}