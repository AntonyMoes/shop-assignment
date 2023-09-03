using System;
using _Game.Scripts.Equipment;
using _Game.Scripts.Objects;
using _Game.Scripts.UI.DragAndDrop;
using GeneralUtils;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class EquipmentPanelSlot : MonoBehaviour {
        [SerializeField] private EquipmentSlot _slot;
        [SerializeField] private GameObject _emptySlotImage;
        [SerializeField] private DropComponent _dropComponent;

        public EquipmentSlot Slot => _slot;

        private readonly Action<IEquipment> _onEquipmentUpdate;
        public readonly Event<IEquipment> OnEquipmentUpdate;

        private InventoryObjectUI _inventoryObject;

        public EquipmentPanelSlot() {
            OnEquipmentUpdate = new Event<IEquipment>(out _onEquipmentUpdate);
        }

        private void Awake() {
            _dropComponent.Init(CanDrop);
            _dropComponent.OnDropped.Subscribe(OnDropped);
        }

        public void Load(EquipmentObject equipment, IInventoryObjectFactory inventoryObjectFactory) {
            if (equipment != null) {
                _inventoryObject = inventoryObjectFactory.CreateInventoryObject(transform, equipment, _dropComponent);
            }
        }

        public void Clear() {
            if (_inventoryObject != null) {
                Destroy(_inventoryObject.gameObject);
            }
        }

        private bool CanDrop(DragComponent component) {
            return component.TryGetComponent<InventoryObjectUI>(out var inventoryObjectUI) &&
                   inventoryObjectUI.InventoryObject is EquipmentObject equipmentObject &&
                   equipmentObject.Slot == _slot;
        }

        private void OnDropped(DragComponent dragComponent) {
            var inventoryObjectUI = dragComponent != null ? dragComponent.GetComponent<InventoryObjectUI>() : null;
            var equipmentObject = inventoryObjectUI != null
                ? inventoryObjectUI.InventoryObject as EquipmentObject
                : null;

            _inventoryObject = inventoryObjectUI;
            _emptySlotImage.SetActive(equipmentObject == null);
            _onEquipmentUpdate(equipmentObject);
        }
    }
}